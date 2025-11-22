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
        private readonly IMapper _mapper;

        public NuotaikaService(INuotaikaRepository nuotaikaRepository, IMapper mapper)
        {
            _nuotaikaRepository = nuotaikaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NuotaikaDto>> GetAllAsync()
        {
            var nuotaikos = await _nuotaikaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NuotaikaDto>>(nuotaikos);
        }

        public async Task<(Result Result, NuotaikaDto? Nuotaika)> CreateAsync(string pavadinimas)
        {
            if (await _nuotaikaRepository.ExistsByNameAsync(pavadinimas))
            {
                return (Result.Failure("Nuotaika su šiuo pavadinimu jau egzistuoja"), null);
            }

            var nuotaika = new Nuotaika { pavadinimas = pavadinimas };
            await _nuotaikaRepository.AddAsync(nuotaika);

            return (Result.Success(), _mapper.Map<NuotaikaDto>(nuotaika));
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
