using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IAutoriusService
    {
        Task<(Result Result, AutoriusDetailDto? Autorius)> GetByIdAsync(Guid id);
        Task<PaginatedResultDto<AutoriusListDto>> GetAllAsync(int page, int pageSize);
        Task<(Result Result, AutoriusDetailDto? Autorius)> CreateAsync(CreateAutoriusDto dto);
        Task<(Result Result, AutoriusDetailDto? Autorius)> UpdateAsync(Guid id, UpdateAutoriusDto dto);
        Task<Result> DeleteAsync(Guid id);
        Task<IEnumerable<KnygaListDto>> GetKnygosAsync(Guid autoriusId);
        Task<IEnumerable<CitataDto>> GetCitatosAsync(Guid autoriusId);
    }
}
