using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Balsavimai;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class BalsavimasService : IBalsavimasService
    {
        private readonly IBalsavimasRepository _balsavimasRepository;
        private readonly IKnygaRepository _knygaRepository;
        private readonly IMeteoService _meteoService;
        private readonly IMapper _mapper;

        public BalsavimasService(
            IBalsavimasRepository balsavimasRepository,
            IKnygaRepository knygaRepository,
            IMeteoService meteoService,
            IMapper mapper)
        {
            _balsavimasRepository = balsavimasRepository;
            _knygaRepository = knygaRepository;
            _meteoService = meteoService;
            _mapper = mapper;
        }

        public async Task<(Result Result, BalsavimasDto? Balsavimas)> GetCurrentAsync()
        {
            var balsavimas = await _balsavimasRepository.GetCurrentAsync();

            if (balsavimas == null)
            {
                return (Result.Failure("Šiuo metu nėra aktyvaus balsavimo"), null);
            }

            return (Result.Success(), await MapToDtoAsync(balsavimas));
        }

        public async Task<(Result Result, BalsavimasDto? Balsavimas)> GetByIdAsync(Guid id)
        {
            var balsavimas = await _balsavimasRepository.GetByIdWithDetailsAsync(id);

            if (balsavimas == null)
            {
                return (Result.Failure(Constants.BalsavimasNerastas), null);
            }

            return (Result.Success(), await MapToDtoAsync(balsavimas));
        }

        public async Task<(Result Result, BalsavimasDto? Balsavimas)> CreateAsync(CreateBalsavimasDto dto)
        {
            var balsavimas = new Balsavimas
            {
                balsavimo_pradzia = dto.balsavimo_pradzia,
                balsavimo_pabaiga = dto.balsavimo_pabaiga,
                susitikimo_data = dto.susitikimo_data
            };

            await _balsavimasRepository.AddAsync(balsavimas);

            // Add initial votes with count 0 for nominated books
            foreach (var knygaId in dto.nominuotos_knygos)
            {
                // We don't add dummy votes, the nominated books will be tracked
                // through the actual votes cast
            }

            var result = await _balsavimasRepository.GetByIdWithDetailsAsync(balsavimas.Id);
            return (Result.Success(), await MapToDtoAsync(result!));
        }

        public async Task<(Result Result, bool Success)> VoteAsync(Guid naudotojasId, CreateBalsasDto dto)
        {
            var balsavimas = await _balsavimasRepository.GetByIdAsync(dto.BalsavimasId);

            if (balsavimas == null)
            {
                return (Result.Failure(Constants.BalsavimasNerastas), false);
            }

            if (balsavimas.uzbaigtas)
            {
                return (Result.Failure(Constants.BalsavimasUzbaigtas), false);
            }

            var now = DateTime.UtcNow;
            if (now < balsavimas.balsavimo_pradzia || now > balsavimas.balsavimo_pabaiga)
            {
                return (Result.Failure("Balsavimas nėra aktyvus"), false);
            }

            if (await _balsavimasRepository.HasVotedAsync(dto.BalsavimasId, naudotojasId))
            {
                return (Result.Failure(Constants.JauBalsavote), false);
            }

            var knyga = await _knygaRepository.GetByIdAsync(dto.KnygaId);
            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), false);
            }

            var balsas = new Balsas
            {
                NaudotojasId = naudotojasId,
                BalsavimasId = dto.BalsavimasId,
                KnygaId = dto.KnygaId
            };

            await _balsavimasRepository.AddBalsasAsync(balsas);

            return (Result.Success(), true);
        }

        public async Task<Result> RemoveVoteAsync(Guid balsasId, Guid naudotojasId)
        {
            var balsas = await _balsavimasRepository.GetBalsasByIdAsync(balsasId);

            if (balsas == null)
            {
                return Result.Failure("Balsas nerastas");
            }

            if (balsas.NaudotojasId != naudotojasId)
            {
                return Result.Failure(Constants.NeturitePrieigos);
            }

            await _balsavimasRepository.DeleteBalsasAsync(balsasId);

            return Result.Success();
        }

        public async Task<(Result Result, string? OroPrognoze)> GetOroPrognozeAsync(Guid balsavimasId)
        {
            var balsavimas = await _balsavimasRepository.GetByIdAsync(balsavimasId);

            if (balsavimas == null)
            {
                return (Result.Failure(Constants.BalsavimasNerastas), null);
            }

            if (!balsavimas.susitikimo_data.HasValue)
            {
                return (Result.Failure("Susitikimo data nenustatyta"), null);
            }

            // Get real weather forecast from api.meteo.lt (not saved to database)
            var weather = await _meteoService.GetOroPrognozeAsync(balsavimas.susitikimo_data.Value);

            return (Result.Success(), weather);
        }

        private async Task<BalsavimasDto> MapToDtoAsync(Balsavimas balsavimas)
        {
            var voteCounts = await _balsavimasRepository.GetVoteCountsAsync(balsavimas.Id);

            var knygosBalsu = new List<KnygaBalsuDto>();

            var uniqueKnygaIds = balsavimas.Balsai?.Select(b => b.KnygaId).Distinct() ?? Enumerable.Empty<Guid>();

            foreach (var knygaId in uniqueKnygaIds)
            {
                var knyga = balsavimas.Balsai?.FirstOrDefault(b => b.KnygaId == knygaId)?.Knyga;
                if (knyga != null)
                {
                    knygosBalsu.Add(new KnygaBalsuDto
                    {
                        Id = knyga.Id,
                        knygos_pavadinimas = knyga.knygos_pavadinimas,
                        virselio_nuotrauka = knyga.virselio_nuotrauka,
                        autorius_vardas = knyga.Autorius != null
                            ? $"{knyga.Autorius.vardas} {knyga.Autorius.pavarde}"
                            : "",
                        balsu_skaicius = voteCounts.GetValueOrDefault(knygaId, 0)
                    });
                }
            }

            return new BalsavimasDto
            {
                Id = balsavimas.Id,
                balsavimo_pradzia = balsavimas.balsavimo_pradzia,
                balsavimo_pabaiga = balsavimas.balsavimo_pabaiga,
                uzbaigtas = balsavimas.uzbaigtas,
                susitikimo_data = balsavimas.susitikimo_data,
                isrinkta_knyga = balsavimas.IsrinktaKnyga != null
                    ? _mapper.Map<DTOs.Knygos.KnygaListDto>(balsavimas.IsrinktaKnyga)
                    : null,
                nominuotos_knygos = knygosBalsu.OrderByDescending(k => k.balsu_skaicius).ToList(),
                viso_balsu = voteCounts.Values.Sum()
            };
        }
    }
}
