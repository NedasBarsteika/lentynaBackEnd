using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface ICitataRepository
    {
        Task<Citata?> GetByIdAsync(Guid id);
        Task<IEnumerable<Citata>> GetByAutoriusIdAsync(Guid autoriusId);
        Task<Citata> AddAsync(Citata citata);
        Task UpdateAsync(Citata citata);
        Task<bool> DeleteAsync(Guid id);
    }
}
