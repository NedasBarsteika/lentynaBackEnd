using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Komentarai;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class KomentarasService : IKomentarasService
    {
        private readonly IKomentarasRepository _komentarasRepository;
        private readonly IKnygaRepository _knygaRepository;
        private readonly ITemaRepository _temaRepository;
        private readonly IMapper _mapper;
        private readonly IAutoriusRepository _autoriusRepository;
        private readonly ISekimasRepository _sekimasRepository;
        private readonly IDIKomentarasRepository _diKomentarasRepository;
        private readonly IOpenAIService _openAIService;
        private readonly IEmailService _emailService;
        private readonly ILogger<KnygaService> _logger;

        public KomentarasService(
            IKomentarasRepository komentarasRepository,
            IKnygaRepository knygaRepository,
            ITemaRepository temaRepository,
            IMapper mapper,
            IAutoriusRepository autoriusRepository,
            ISekimasRepository sekimasRepository,
            IDIKomentarasRepository diKomentarasRepository,
            IOpenAIService openAIService,
            IEmailService emailService,
            ILogger<KnygaService> logger)
        {
            _komentarasRepository = komentarasRepository;
            _knygaRepository = knygaRepository;
            _temaRepository = temaRepository;
            _mapper = mapper;
            _autoriusRepository = autoriusRepository;
            _sekimasRepository = sekimasRepository;
            _diKomentarasRepository = diKomentarasRepository;
            _openAIService = openAIService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IEnumerable<KomentarasDto>> GetByKnygaIdAsync(Guid knygaId)
        {
            var komentarai = await _komentarasRepository.GetByKnygaIdAsync(knygaId);
            return _mapper.Map<IEnumerable<KomentarasDto>>(komentarai);
        }

        public async Task<IEnumerable<KomentarasDto>> GetByTemaIdAsync(Guid temaId)
        {
            var komentarai = await _komentarasRepository.GetByTemaIdAsync(temaId);
            return _mapper.Map<IEnumerable<KomentarasDto>>(komentarai);
        }

        public async Task<(Result Result, KomentarasDto? Komentaras)> CreateAsync(Guid naudotojasId, CreateKomentarasDto dto)
        {
            if (dto.KnygaId.HasValue)
            {
                var knyga = await _knygaRepository.GetByIdAsync(dto.KnygaId.Value);
                if (knyga == null)
                {
                    return (Result.Failure(Constants.KnygaNerastas), null);
                }
            }

            if (dto.TemaId.HasValue)
            {
                var tema = await _temaRepository.GetByIdAsync(dto.TemaId.Value);
                if (tema == null)
                {
                    return (Result.Failure(Constants.TemaNerastas), null);
                }
            }

            var komentaras = new Komentaras
            {
                komentaro_tekstas = dto.komentaro_tekstas,
                vertinimas = dto.vertinimas,
                NaudotojasId = naudotojasId,
                KnygaId = dto.KnygaId,
                TemaId = dto.TemaId
            };

            await _komentarasRepository.AddAsync(komentaras);

            var result = await _komentarasRepository.GetByIdAsync(komentaras.Id);
            return (Result.Success(), _mapper.Map<KomentarasDto>(result));
        }

        public async Task<(Result Result, KomentarasDto? Komentaras)> UpdateAsync(Guid id, Guid naudotojasId, UpdateKomentarasDto dto)
        {
            var komentaras = await _komentarasRepository.GetByIdAsync(id);

            if (komentaras == null)
            {
                return (Result.Failure(Constants.KomentarasNerastas), null);
            }

            if (komentaras.NaudotojasId != naudotojasId)
            {
                return (Result.Failure(Constants.NeturitePrieigos), null);
            }

            if (!string.IsNullOrEmpty(dto.komentaro_tekstas))
            {
                komentaras.komentaro_tekstas = dto.komentaro_tekstas;
            }

            if (dto.vertinimas.HasValue)
            {
                komentaras.vertinimas = dto.vertinimas.Value;
            }

            await _komentarasRepository.UpdateAsync(komentaras);

            var result = await _komentarasRepository.GetByIdAsync(id);
            return (Result.Success(), _mapper.Map<KomentarasDto>(result));
        }


        public async Task<(Result Result, DIKomentarasDto? Knyga)> GetDIComment(Guid id)
        {

            _logger.LogInformation(null, "generating/updating DI comment for book");
            var knyga = await _knygaRepository.GetByIdWithDetailsAsync(id);

            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), null);
            }

            // Check if DI comment needs regeneration
            var dikom = await GenerateOrUpdateDICommentIfNeededAsync(id, knyga.knygos_pavadinimas);

            _logger.LogInformation(null, "Dikom {dikom}", dikom);

            // Reload knyga to get updated DI comment
            var dto = _mapper.Map<DIKomentarasDto>(dikom);

            return (Result.Success(), dto);
        }

        private async Task<Dirbtinio_intelekto_komentaras?> GenerateOrUpdateDICommentIfNeededAsync(Guid knygaId, string knygosPavadinimas)
        {
            try
            {
                // Get all reviews for this book
                var komentarai = (await _komentarasRepository.GetByKnygaIdAsync(knygaId))
                    .Where(k => k.Naudotojas != null)
                    .ToList();

                var existingDIComment = await _diKomentarasRepository.GetByKnygaIdAsync(knygaId);

                var now = DateTime.Now;

                if (existingDIComment == null || existingDIComment.sugeneravimo_data.AddDays(7) < now || existingDIComment.tekstas == "Atsiprašome, nepavyko sugeneruoti automatinio atsiliepimo. Bandykite vėliau." || existingDIComment.tekstas == "Kol kas nėra jokių atsiliepimų apie šią knygą. Būkite pirmas, kuris pasidalins savo nuomone!")
                {
                    var aiReview = await _openAIService.GeneruotiKnygosAtsiliepima(knygosPavadinimas, komentarai);

                    var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

                    // Create new comment
                    var newDIComment = new Dirbtinio_intelekto_komentaras
                    {
                        KnygaId = knygaId,
                        tekstas = aiReview,
                        modelis = model
                    };
                    var komentaras = await _diKomentarasRepository.AddAsync(newDIComment);

                    _logger.LogInformation("Created new DI comment for book {KnygaId}", knygaId);
                    return komentaras;
                }

                return existingDIComment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating/updating DI comment for book {KnygaId}", knygaId);
                // Don't throw - we don't want to break the GetById operation if AI generation fails
                return null;
            }
        }


        public async Task<Result> DeleteAsync(Guid id, Guid naudotojasId, bool isAdmin)
        {
            var komentaras = await _komentarasRepository.GetByIdAsync(id);

            if (komentaras == null)
            {
                return Result.Failure(Constants.KomentarasNerastas);
            }

            if (komentaras.NaudotojasId != naudotojasId && !isAdmin)
            {
                return Result.Failure(Constants.NeturitePrieigos);
            }

            await _komentarasRepository.DeleteAsync(id);

            return Result.Success();
        }
    }
}
