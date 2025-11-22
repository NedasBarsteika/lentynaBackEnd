using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.DTOs.Citatos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface ICitataService
    {
        Task<IEnumerable<CitataDto>> GetByAutoriusIdAsync(Guid autoriusId);
        Task<(Result Result, CitataDto? Citata)> CreateAsync(CreateCitataDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
}
