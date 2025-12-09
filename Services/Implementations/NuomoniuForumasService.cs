using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.NuomoniuForumas;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class NuomoniuForumasService : INuomoniuForumasService
    {
        private readonly INuomoniuForumasRepository _nuomoniuForumasRepository;
        private readonly IMapper _mapper;

        public NuomoniuForumasService(INuomoniuForumasRepository nuomoniuForumasRepository, IMapper mapper)
        {
            _nuomoniuForumasRepository = nuomoniuForumasRepository;
            _mapper = mapper;
        }

        public async Task<(Result Result, TemaDetailDto? Tema)> GetByIdAsync(Guid id)
        {
            var tema = await _nuomoniuForumasRepository.GetByIdWithDetailsAsync(id);

            if (tema == null)
            {
                return (Result.Failure(Constants.TemaNerastas), null);
            }

            return (Result.Success(), _mapper.Map<TemaDetailDto>(tema));
        }

        public async Task<PaginatedResultDto<TemaListDto>> GetAllAsync(int page, int pageSize)
        {
            var (items, totalCount) = await _nuomoniuForumasRepository.GetAllAsync(page, pageSize);
            var dtos = _mapper.Map<List<TemaListDto>>(items);

            return new PaginatedResultDto<TemaListDto>(dtos, page, pageSize, totalCount);
        }

        public async Task<(Result Result, TemaDetailDto? Tema)> CreateAsync(Guid naudotojasId, CreateTemaDto dto)
        {
            var tema = new Tema
            {
                pavadinimas = dto.pavadinimas,
                tekstas = dto.tekstas,
                NaudotojasId = naudotojasId
            };

            await _nuomoniuForumasRepository.AddAsync(tema);

            var result = await _nuomoniuForumasRepository.GetByIdWithDetailsAsync(tema.Id);
            return (Result.Success(), _mapper.Map<TemaDetailDto>(result));
        }

        public async Task<(Result Result, TemaDetailDto? Tema)> UpdateAsync(Guid id, Guid naudotojasId, UpdateTemaDto dto)
        {
            var tema = await _nuomoniuForumasRepository.GetByIdAsync(id);

            if (tema == null)
            {
                return (Result.Failure(Constants.TemaNerastas), null);
            }

            if (tema.NaudotojasId != naudotojasId)
            {
                return (Result.Failure(Constants.NeturitePrieigos), null);
            }

            if (!string.IsNullOrEmpty(dto.pavadinimas)) tema.pavadinimas = dto.pavadinimas;
            if (!string.IsNullOrEmpty(dto.tekstas)) tema.tekstas = dto.tekstas;

            await _nuomoniuForumasRepository.UpdateAsync(tema);

            var result = await _nuomoniuForumasRepository.GetByIdWithDetailsAsync(id);
            return (Result.Success(), _mapper.Map<TemaDetailDto>(result));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid naudotojasId, bool isModerator)
        {
            var tema = await _nuomoniuForumasRepository.GetByIdAsync(id);

            if (tema == null)
            {
                return Result.Failure(Constants.TemaNerastas);
            }

            if (tema.NaudotojasId != naudotojasId && !isModerator)
            {
                return Result.Failure(Constants.NeturitePrieigos);
            }

            await _nuomoniuForumasRepository.SoftDeleteAsync(id);

            return Result.Success();
        }
    }
}
