using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class ZanrasService : IZanrasService
    {
        private readonly IZanrasRepository _zanrasRepository;
        private readonly IMapper _mapper;

        public ZanrasService(IZanrasRepository zanrasRepository, IMapper mapper)
        {
            _zanrasRepository = zanrasRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ZanrasDto>> GetAllAsync()
        {
            var zanrai = await _zanrasRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ZanrasDto>>(zanrai);
        }

        public async Task<(Result Result, ZanrasDto? Zanras)> CreateAsync(string pavadinimas)
        {
            if (await _zanrasRepository.ExistsByNameAsync(pavadinimas))
            {
                return (Result.Failure("Žanras su šiuo pavadinimu jau egzistuoja"), null);
            }

            var zanras = new Zanras { pavadinimas = pavadinimas };
            await _zanrasRepository.AddAsync(zanras);

            return (Result.Success(), _mapper.Map<ZanrasDto>(zanras));
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var success = await _zanrasRepository.DeleteAsync(id);

            if (!success)
            {
                return Result.Failure(Constants.ZanrasNerastas);
            }

            return Result.Success();
        }
    }
}
