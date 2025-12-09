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

            var now = DateTime.Now;
            var balsavimasDto = await MapToDtoAsync(balsavimas);
            if (balsavimas.balsavimo_pabaiga < now || (balsavimas.uzbaigtas && balsavimas.isrinkta_knyga_id == null))
            {
                int max = -1;
                Guid? maxId = null;
                KnygaBalsuDto? winningKnyga = null;
                foreach (var knyga in balsavimasDto.nominuotos_knygos)
                {
                    if (knyga.balsu_skaicius > max)
                    {
                        max = knyga.balsu_skaicius;
                        maxId = knyga.Id;
                        winningKnyga = knyga;
                    }
                }
                balsavimas.isrinkta_knyga_id = maxId;
                balsavimas.uzbaigtas = true;
                await _balsavimasRepository.UpdateAsync(balsavimas);

                // Update the DTO with the winning book
                balsavimasDto.uzbaigtas = true;
                balsavimasDto.isrinkta_knyga = winningKnyga;
                return (Result.Success(), balsavimasDto);
            }

            return (Result.Success(), balsavimasDto);
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
            };

            await _balsavimasRepository.AddAsync(balsavimas);

            // Add nominated books to the many-to-many relationship
            foreach (var knygaId in dto.nominuotos_knygos)
            {
                var knyga = await _knygaRepository.GetByIdAsync(knygaId);
                if (knyga != null)
                {
                    var balsavimoKnyga = new BalsavimoKnyga
                    {
                        BalsavimasId = balsavimas.Id,
                        KnygaId = knygaId
                    };
                    await _balsavimasRepository.AddBalsavimoKnygaAsync(balsavimoKnyga);
                }
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

            // Check if the book is nominated for this voting session
            if (!await _balsavimasRepository.IsKnygaNominuotaAsync(dto.BalsavimasId, dto.KnygaId))
            {
                return (Result.Failure("Ši knyga nėra nominuota šiame balsavime"), false);
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

            var susitikimo_data = balsavimas.balsavimo_pabaiga.AddDays(2);
            var weather = await _meteoService.GetOroPrognozeAsync(susitikimo_data);

            return (Result.Success(), weather);
        }

        private async Task<BalsavimasDto> MapToDtoAsync(Balsavimas balsavimas)
        {
            var voteCounts = await _balsavimasRepository.GetVoteCountsAsync(balsavimas.Id);

            var knygosBalsu = new List<KnygaBalsuDto>();

            // Use BalsavimoKnygos for nominated books instead of deriving from votes
            foreach (var balsavimoKnyga in balsavimas.BalsavimoKnygos ?? new List<BalsavimoKnyga>())
            {
                var knyga = balsavimoKnyga.Knyga;
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
                        balsu_skaicius = voteCounts.GetValueOrDefault(knyga.Id, 0)
                    });
                }
            }

            return new BalsavimasDto
            {
                Id = balsavimas.Id,
                balsavimo_pradzia = balsavimas.balsavimo_pradzia,
                balsavimo_pabaiga = balsavimas.balsavimo_pabaiga,
                uzbaigtas = balsavimas.uzbaigtas,
                isrinkta_knyga = balsavimas.IsrinktaKnyga != null ? new KnygaBalsuDto
                {
                    Id = balsavimas.IsrinktaKnyga.Id,
                    knygos_pavadinimas = balsavimas.IsrinktaKnyga.knygos_pavadinimas,
                    virselio_nuotrauka = balsavimas.IsrinktaKnyga.virselio_nuotrauka,
                    autorius_vardas = balsavimas.IsrinktaKnyga.Autorius != null
                            ? $"{balsavimas.IsrinktaKnyga.Autorius.vardas} {balsavimas.IsrinktaKnyga.Autorius.pavarde}"
                            : "",
                    balsu_skaicius = voteCounts.GetValueOrDefault(balsavimas.IsrinktaKnyga.Id, 0)
                } : null,
                nominuotos_knygos = knygosBalsu.OrderByDescending(k => k.balsu_skaicius).ToList(),
                viso_balsu = voteCounts.Values.Sum()
            };
        }
    }
}
