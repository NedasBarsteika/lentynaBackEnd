using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class KnygaService : IKnygaService
    {
        private readonly IKnygaRepository _knygaRepository;
        private readonly IAutoriusRepository _autoriusRepository;
        private readonly IKomentarasRepository _komentarasRepository;
        private readonly IDIKomentarasRepository _diKomentarasRepository;
        private readonly IOpenAIService _openAIService;
        private readonly IMapper _mapper;

        public KnygaService(
            IKnygaRepository knygaRepository,
            IAutoriusRepository autoriusRepository,
            IKomentarasRepository komentarasRepository,
            IDIKomentarasRepository diKomentarasRepository,
            IOpenAIService openAIService,
            IMapper mapper)
        {
            _knygaRepository = knygaRepository;
            _autoriusRepository = autoriusRepository;
            _komentarasRepository = komentarasRepository;
            _diKomentarasRepository = diKomentarasRepository;
            _openAIService = openAIService;
            _mapper = mapper;
        }

        public async Task<(Result Result, KnygaDetailDto? Knyga)> GetByIdAsync(Guid id)
        {
            var knyga = await _knygaRepository.GetByIdWithDetailsAsync(id);

            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), null);
            }

            // Reload knyga to get updated DI comment
            knyga = await _knygaRepository.GetByIdWithDetailsAsync(id);

            var dto = _mapper.Map<KnygaDetailDto>(knyga);
            dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(id);
            dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(id);

            return (Result.Success(), dto);
        }

        public async Task<PaginatedResultDto<KnygaListDto>> GetAllAsync(KnygaFilterDto filter)
        {
            var (items, totalCount) = await _knygaRepository.GetAllAsync(filter);

            var dtos = new List<KnygaListDto>();
            foreach (var knyga in items)
            {
                var dto = _mapper.Map<KnygaListDto>(knyga);
                dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                dtos.Add(dto);
            }

            return new PaginatedResultDto<KnygaListDto>(dtos, filter.page, filter.pageSize, totalCount);
        }

        public async Task<(Result Result, KnygaDetailDto? Knyga)> CreateAsync(CreateKnygaDto dto)
        {
            var autorius = await _autoriusRepository.GetByIdAsync(dto.AutoriusId);
            if (autorius == null)
            {
                return (Result.Failure(Constants.AutoriusNerastas), null);
            }

            var knyga = new Knyga
            {
                knygos_pavadinimas = dto.knygos_pavadinimas,
                leidimo_metai = dto.leidimo_metai,
                aprasymas = dto.aprasymas,
                psl_skaicius = dto.psl_skaicius,
                ISBN = dto.ISBN,
                virselio_nuotrauka = dto.virselio_nuotrauka,
                kalba = dto.kalba,
                bestseleris = dto.bestseleris,
                AutoriusId = dto.AutoriusId,
                ZanrasId = dto.ZanrasId
            };

            await _knygaRepository.AddAsync(knyga);
            await _autoriusRepository.UpdateKnyguSkaicius(dto.AutoriusId);

            return await GetByIdAsync(knyga.Id);
        }

        public async Task<(Result Result, KnygaDetailDto? Knyga)> UpdateAsync(Guid id, UpdateKnygaDto dto)
        {
            var knyga = await _knygaRepository.GetByIdAsync(id);

            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), null);
            }

            var oldAutoriusId = knyga.AutoriusId;

            if (!string.IsNullOrEmpty(dto.knygos_pavadinimas)) knyga.knygos_pavadinimas = dto.knygos_pavadinimas;
            if (dto.leidimo_metai.HasValue) knyga.leidimo_metai = dto.leidimo_metai;
            if (dto.aprasymas != null) knyga.aprasymas = dto.aprasymas;
            if (dto.psl_skaicius.HasValue) knyga.psl_skaicius = dto.psl_skaicius;
            if (dto.ISBN != null) knyga.ISBN = dto.ISBN;
            if (dto.virselio_nuotrauka != null) knyga.virselio_nuotrauka = dto.virselio_nuotrauka;
            if (dto.kalba != null) knyga.kalba = dto.kalba;
            if (dto.bestseleris.HasValue) knyga.bestseleris = dto.bestseleris.Value;
            if (dto.ZanrasId.HasValue) knyga.ZanrasId = dto.ZanrasId.Value;

            if (dto.AutoriusId.HasValue && dto.AutoriusId.Value != knyga.AutoriusId)
            {
                var newAutorius = await _autoriusRepository.GetByIdAsync(dto.AutoriusId.Value);
                if (newAutorius == null)
                {
                    return (Result.Failure(Constants.AutoriusNerastas), null);
                }
                knyga.AutoriusId = dto.AutoriusId.Value;
            }

            await _knygaRepository.UpdateAsync(knyga);

            if (dto.AutoriusId.HasValue && dto.AutoriusId.Value != oldAutoriusId)
            {
                await _autoriusRepository.UpdateKnyguSkaicius(oldAutoriusId);
                await _autoriusRepository.UpdateKnyguSkaicius(dto.AutoriusId.Value);
            }

            return await GetByIdAsync(id);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var knyga = await _knygaRepository.GetByIdAsync(id);
            if (knyga == null)
            {
                return Result.Failure(Constants.KnygaNerastas);
            }

            var autoriusId = knyga.AutoriusId;

            var success = await _knygaRepository.DeleteAsync(id);
            if (!success)
            {
                return Result.Failure(Constants.KnygaNerastas);
            }

            await _autoriusRepository.UpdateKnyguSkaicius(autoriusId);

            return Result.Success();
        }

        public async Task<List<KnygaListDto>> IsplestinePaieskaAsync(IsplestinePaieskaDto dto)
        {
            // Check if any filter is provided
            var hasZanruFilter = dto.ZanruIds != null && dto.ZanruIds.Count > 0;
            var hasNuotaikuFilter = dto.NuotaikuIds != null && dto.NuotaikuIds.Count > 0;
            var hasScenarijusFilter = !string.IsNullOrWhiteSpace(dto.ScenarijausAprasymas);

            IEnumerable<Knyga> knygosList;

            // Get books filtered by genres and/or moods
            if (hasZanruFilter || hasNuotaikuFilter)
            {
                knygosList = await _knygaRepository.GetBooksForAdvancedSearchAsync(dto.ZanruIds, dto.NuotaikuIds);
            }
            else
            {
                // No genre/mood filters - get all books
                var filter = new KnygaFilterDto { page = 1, pageSize = 100 };
                var (allKnygos, _) = await _knygaRepository.GetAllAsync(filter);
                knygosList = allKnygos;
            }

            var knygosListMaterialized = knygosList.ToList();

            // If no scenario description, just return filtered results
            if (!hasScenarijusFilter)
            {
                var result = new List<KnygaListDto>();
                foreach (var knyga in knygosListMaterialized.Take(10))
                {
                    var knygaDto = _mapper.Map<KnygaListDto>(knyga);
                    knygaDto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                    knygaDto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                    result.Add(knygaDto);
                }
                return result;
            }

            // Prepare data for AI
            var knyguSearchDtos = knygosListMaterialized.Select(k => new KnygaSearchDto
            {
                Id = k.Id,
                Pavadinimas = k.knygos_pavadinimas,
                Aprasymas = k.aprasymas,
                Zanras = k.Zanras?.pavadinimas ?? ""
            }).ToList();

            // Call AI to find matching books based on scenario description
            var matchingIds = await _openAIService.IeskoitKnyguPagalAprasymaAsync(
                dto.ScenarijausAprasymas!,
                knyguSearchDtos);

            // Get matching books in order returned by AI
            var matchedKnygos = new List<KnygaListDto>();
            foreach (var matchId in matchingIds)
            {
                var knyga = knygosListMaterialized.FirstOrDefault(k => k.Id == matchId);
                if (knyga != null)
                {
                    var knygaDto = _mapper.Map<KnygaListDto>(knyga);
                    knygaDto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                    knygaDto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                    matchedKnygos.Add(knygaDto);
                }
            }

            return matchedKnygos;
        }
    }
}
