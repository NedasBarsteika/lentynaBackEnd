using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.Temos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface ITemaService
    {
        Task<(Result Result, TemaDetailDto? Tema)> GetByIdAsync(Guid id);
        Task<PaginatedResultDto<TemaListDto>> GetAllAsync(int page, int pageSize);
        Task<(Result Result, TemaDetailDto? Tema)> CreateAsync(Guid naudotojasId, CreateTemaDto dto);
        Task<(Result Result, TemaDetailDto? Tema)> UpdateAsync(Guid id, Guid naudotojasId, UpdateTemaDto dto);
        Task<Result> DeleteAsync(Guid id, Guid naudotojasId, bool isModerator);
    }
}
