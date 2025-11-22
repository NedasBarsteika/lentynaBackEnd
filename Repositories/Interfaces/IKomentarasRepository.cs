using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IKomentarasRepository
    {
        Task<Komentaras?> GetByIdAsync(Guid id);
        Task<IEnumerable<Komentaras>> GetByKnygaIdAsync(Guid knygaId);
        Task<IEnumerable<Komentaras>> GetByTemaIdAsync(Guid temaId);
        Task<IEnumerable<Komentaras>> GetByNaudotojasIdAsync(Guid naudotojasId);
        Task<Komentaras> AddAsync(Komentaras komentaras);
        Task UpdateAsync(Komentaras komentaras);
        Task<bool> DeleteAsync(Guid id);
    }
}
