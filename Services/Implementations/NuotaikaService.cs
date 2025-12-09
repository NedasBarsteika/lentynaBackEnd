using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class NuotaikaService : INuotaikaService
    {
        private readonly INuotaikaRepository _nuotaikaRepository;
        private readonly IZanrasRepository _zanrasRepository;
        private readonly IMapper _mapper;

        public NuotaikaService(INuotaikaRepository nuotaikaRepository, IZanrasRepository zanrasRepository, IMapper mapper)
        {
            _nuotaikaRepository = nuotaikaRepository;
            _zanrasRepository = zanrasRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NuotaikaDto>> GetAllAsync()
        {
            var nuotaikos = await _nuotaikaRepository.GetAllAsync();
            var dtos = new List<NuotaikaDto>();

            foreach (var nuotaika in nuotaikos)
            {
                var dto = new NuotaikaDto
                {
                    Id = nuotaika.Id,
                    pavadinimas = nuotaika.pavadinimas,
                    Zanrai = nuotaika.NuotaikosZanrai
                        .Select(nz => new ZanrasDto
                        {
                            Id = nz.Zanras.Id,
                            pavadinimas = nz.Zanras.pavadinimas
                        })
                        .ToList()
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<(Result Result, NuotaikaDto? Nuotaika)> CreateAsync(string pavadinimas, List<Guid> zanrasIds)
        {
            if (await _nuotaikaRepository.ExistsByNameAsync(pavadinimas))
            {
                return (Result.Failure("Nuotaika su šiuo pavadinimu jau egzistuoja"), null);
            }

            // Validuoti, kad visi žanrai egzistuoja
            var zanraiList = new List<Zanras>();
            if (zanrasIds != null && zanrasIds.Any())
            {
                foreach (var zanrasId in zanrasIds)
                {
                    var zanras = await _zanrasRepository.GetByIdAsync(zanrasId);
                    if (zanras == null)
                    {
                        return (Result.Failure($"Žanras su ID {zanrasId} nerastas"), null);
                    }
                    zanraiList.Add(zanras);
                }
            }

            var nuotaika = new Nuotaika
            {
                pavadinimas = pavadinimas
            };

            await _nuotaikaRepository.AddAsync(nuotaika);

            // Add the many-to-many relationships
            foreach (var zanrasId in zanrasIds ?? new List<Guid>())
            {
                var nuotaikosZanras = new NuotaikosZanras
                {
                    NuotaikaId = nuotaika.Id,
                    ZanrasId = zanrasId
                };
                await _nuotaikaRepository.AddNuotaikosZanrasAsync(nuotaikosZanras);
            }

            // Grąžinti DTO su užkrautais Zanras objektais
            var createdDto = new NuotaikaDto
            {
                Id = nuotaika.Id,
                pavadinimas = nuotaika.pavadinimas,
                Zanrai = zanraiList.Select(z => new ZanrasDto
                {
                    Id = z.Id,
                    pavadinimas = z.pavadinimas
                }).ToList()
            };

            return (Result.Success(), createdDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var success = await _nuotaikaRepository.DeleteAsync(id);

            if (!success)
            {
                return Result.Failure(Constants.NuotaikaNerastas);
            }

            return Result.Success();
        }
    }
}
