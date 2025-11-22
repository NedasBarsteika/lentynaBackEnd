using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Irasai;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IIrasasService
    {
        Task<IEnumerable<IrasasDto>> GetByNaudotojasIdAsync(Guid naudotojasId);
        Task<IEnumerable<IrasasDto>> GetByNaudotojasIdAndTipasAsync(Guid naudotojasId, BookshelfTypes tipas);
        Task<(Result Result, IrasasDto? Irasas)> CreateAsync(Guid naudotojasId, CreateIrasasDto dto);
        Task<(Result Result, IrasasDto? Irasas)> UpdateAsync(Guid id, Guid naudotojasId, UpdateIrasasDto dto);
        Task<Result> DeleteAsync(Guid id, Guid naudotojasId);
        Task<IEnumerable<KnygaListDto>> GetRekomendacijosAsync(Guid naudotojasId);
    }
}
