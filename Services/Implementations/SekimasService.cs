using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Sekimai;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class SekimasService : ISekimasService
    {
        private readonly ISekimasRepository _sekimasRepository;
        private readonly IAutoriusRepository _autoriusRepository;
        private readonly IMapper _mapper;

        public SekimasService(
            ISekimasRepository sekimasRepository,
            IAutoriusRepository autoriusRepository,
            IMapper mapper)
        {
            _sekimasRepository = sekimasRepository;
            _autoriusRepository = autoriusRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SekimasDto>> GetByNaudotojasIdAsync(Guid naudotojasId)
        {
            var sekimai = await _sekimasRepository.GetByNaudotojasIdAsync(naudotojasId);
            return _mapper.Map<IEnumerable<SekimasDto>>(sekimai);
        }

        public async Task<bool> IsFollowingAsync(Guid naudotojasId, Guid autoriusId)
        {
            return await _sekimasRepository.ExistsAsync(naudotojasId, autoriusId);
        }

        public async Task<(Result Result, SekimasDto? Sekimas)> FollowAsync(Guid naudotojasId, CreateSekimasDto dto)
        {
            var autorius = await _autoriusRepository.GetByIdAsync(dto.AutoriusId);
            if (autorius == null)
            {
                return (Result.Failure(Constants.AutoriusNerastas), null);
            }

            if (await _sekimasRepository.ExistsAsync(naudotojasId, dto.AutoriusId))
            {
                return (Result.Failure(Constants.JauSekateAutoriu), null);
            }

            var sekimas = new Autoriaus_sekimas
            {
                NaudotojasId = naudotojasId,
                AutoriusId = dto.AutoriusId
            };

            await _sekimasRepository.AddAsync(sekimas);

            var result = await _sekimasRepository.GetByIdsAsync(naudotojasId, dto.AutoriusId);
            return (Result.Success(), _mapper.Map<SekimasDto>(result));
        }

        public async Task<Result> UnfollowAsync(Guid naudotojasId, Guid autoriusId)
        {
            if (!await _sekimasRepository.ExistsAsync(naudotojasId, autoriusId))
            {
                return Result.Failure("Jūs nesekate šio autoriaus");
            }

            await _sekimasRepository.DeleteAsync(naudotojasId, autoriusId);

            return Result.Success();
        }
    }
}
