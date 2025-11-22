using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IKnygaService
    {
        Task<(Result Result, KnygaDetailDto? Knyga)> GetByIdAsync(Guid id);
        Task<PaginatedResultDto<KnygaListDto>> GetAllAsync(KnygaFilterDto filter);
        Task<(Result Result, KnygaDetailDto? Knyga)> CreateAsync(CreateKnygaDto dto);
        Task<(Result Result, KnygaDetailDto? Knyga)> UpdateAsync(Guid id, UpdateKnygaDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
}
