using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class AutoriusService : IAutoriusService
    {
        private readonly IAutoriusRepository _autoriusRepository;
        private readonly ISekimasRepository _sekimasRepository;
        private readonly IKnygaRepository _knygaRepository;
        private readonly IMapper _mapper;

        public AutoriusService(
            IAutoriusRepository autoriusRepository,
            ISekimasRepository sekimasRepository,
            IKnygaRepository knygaRepository,
            IMapper mapper)
        {
            _autoriusRepository = autoriusRepository;
            _sekimasRepository = sekimasRepository;
            _knygaRepository = knygaRepository;
            _mapper = mapper;
        }

        public async Task<(Result Result, AutoriusDetailDto? Autorius)> GetByIdAsync(Guid id)
        {
            var autorius = await _autoriusRepository.GetByIdWithDetailsAsync(id);

            if (autorius == null)
            {
                return (Result.Failure(Constants.AutoriusNerastas), null);
            }

            var dto = _mapper.Map<AutoriusDetailDto>(autorius);
            dto.sekejuSkaicius = await _sekimasRepository.GetFollowerCountAsync(id);

            return (Result.Success(), dto);
        }

        public async Task<PaginatedResultDto<AutoriusListDto>> GetAllAsync(int page, int pageSize)
        {
            var autoriai = await _autoriusRepository.GetAllAsync(page, pageSize);
            var totalCount = await _autoriusRepository.GetTotalCountAsync();

            var items = _mapper.Map<List<AutoriusListDto>>(autoriai);

            return new PaginatedResultDto<AutoriusListDto>(items, page, pageSize, totalCount);
        }

        public async Task<(Result Result, AutoriusDetailDto? Autorius)> CreateAsync(CreateAutoriusDto dto)
        {
            var autorius = _mapper.Map<Autorius>(dto);
            await _autoriusRepository.AddAsync(autorius);

            var result = await _autoriusRepository.GetByIdWithDetailsAsync(autorius.Id);
            return (Result.Success(), _mapper.Map<AutoriusDetailDto>(result));
        }

        public async Task<(Result Result, AutoriusDetailDto? Autorius)> UpdateAsync(Guid id, UpdateAutoriusDto dto)
        {
            var autorius = await _autoriusRepository.GetByIdAsync(id);

            if (autorius == null)
            {
                return (Result.Failure(Constants.AutoriusNerastas), null);
            }

            if (!string.IsNullOrEmpty(dto.vardas)) autorius.vardas = dto.vardas;
            if (!string.IsNullOrEmpty(dto.pavarde)) autorius.pavarde = dto.pavarde;
            if (dto.gimimo_metai.HasValue) autorius.gimimo_metai = dto.gimimo_metai;
            if (dto.mirties_data.HasValue) autorius.mirties_data = dto.mirties_data;
            if (dto.curiculum_vitae != null) autorius.curiculum_vitae = dto.curiculum_vitae;
            if (dto.nuotrauka != null) autorius.nuotrauka = dto.nuotrauka;
            if (dto.tautybe != null) autorius.tautybe = dto.tautybe;

            await _autoriusRepository.UpdateAsync(autorius);

            var result = await _autoriusRepository.GetByIdWithDetailsAsync(id);
            var resultDto = _mapper.Map<AutoriusDetailDto>(result);
            resultDto.sekejuSkaicius = await _sekimasRepository.GetFollowerCountAsync(id);

            return (Result.Success(), resultDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var success = await _autoriusRepository.DeleteAsync(id);

            if (!success)
            {
                return Result.Failure(Constants.AutoriusNerastas);
            }

            return Result.Success();
        }

        public async Task<IEnumerable<KnygaListDto>> GetKnygosAsync(Guid autoriusId)
        {
            var knygos = await _autoriusRepository.GetKnygosAsync(autoriusId);
            var result = new List<KnygaListDto>();

            foreach (var knyga in knygos)
            {
                var dto = _mapper.Map<KnygaListDto>(knyga);
                dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                result.Add(dto);
            }

            return result;
        }

        public async Task<IEnumerable<CitataDto>> GetCitatosAsync(Guid autoriusId)
        {
            var citatos = await _autoriusRepository.GetCitatosAsync(autoriusId);
            return _mapper.Map<IEnumerable<CitataDto>>(citatos);
        }
    }
}
