using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Komentarai;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IKomentarasService
    {
        Task<IEnumerable<KomentarasDto>> GetByKnygaIdAsync(Guid knygaId);
        Task<IEnumerable<KomentarasDto>> GetByTemaIdAsync(Guid temaId);
        Task<(Result Result, KomentarasDto? Komentaras)> CreateAsync(Guid naudotojasId, CreateKomentarasDto dto);
        Task<(Result Result, KomentarasDto? Komentaras)> UpdateAsync(Guid id, Guid naudotojasId, UpdateKomentarasDto dto);
        Task<Result> DeleteAsync(Guid id, Guid naudotojasId, bool isAdmin);
        Task<(Result Result, DIKomentarasDto? Knyga)> GetDIComment(Guid id);
    }
}
