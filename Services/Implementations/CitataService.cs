using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.DTOs.Citatos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class CitataService : ICitataService
    {
        private readonly ICitataRepository _citataRepository;
        private readonly IAutoriusRepository _autoriusRepository;
        private readonly IMapper _mapper;

        public CitataService(
            ICitataRepository citataRepository,
            IAutoriusRepository autoriusRepository,
            IMapper mapper)
        {
            _citataRepository = citataRepository;
            _autoriusRepository = autoriusRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CitataDto>> GetByAutoriusIdAsync(Guid autoriusId)
        {
            var citatos = await _citataRepository.GetByAutoriusIdAsync(autoriusId);
            return _mapper.Map<IEnumerable<CitataDto>>(citatos);
        }

        public async Task<(Result Result, CitataDto? Citata)> CreateAsync(CreateCitataDto dto)
        {
            var autorius = await _autoriusRepository.GetByIdAsync(dto.AutoriusId);
            if (autorius == null)
            {
                return (Result.Failure(Constants.AutoriusNerastas), null);
            }

            var citata = new Citata
            {
                citatos_tekstas = dto.citatos_tekstas,
                citatos_data = dto.citatos_data,
                citatos_saltinis = dto.citatos_saltinis,
                AutoriusId = dto.AutoriusId
            };

            await _citataRepository.AddAsync(citata);

            return (Result.Success(), _mapper.Map<CitataDto>(citata));
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var success = await _citataRepository.DeleteAsync(id);

            if (!success)
            {
                return Result.Failure("Citata nerasta");
            }

            return Result.Success();
        }
    }
}
