using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IDIKomentarasRepository
    {
        Task<Dirbtinio_intelekto_komentaras?> GetByKnygaIdAsync(Guid knygaId);
        Task<Dirbtinio_intelekto_komentaras> AddAsync(Dirbtinio_intelekto_komentaras diKomentaras);
        Task UpdateAsync(Dirbtinio_intelekto_komentaras diKomentaras);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> NeedsRegenerationAsync(Guid knygaId);
    }
}
