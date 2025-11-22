using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface ISekimasRepository
    {
        Task<IEnumerable<Autoriaus_sekimas>> GetByNaudotojasIdAsync(Guid naudotojasId);
        Task<Autoriaus_sekimas?> GetByIdsAsync(Guid naudotojasId, Guid autoriusId);
        Task<Autoriaus_sekimas> AddAsync(Autoriaus_sekimas sekimas);
        Task<bool> DeleteAsync(Guid naudotojasId, Guid autoriusId);
        Task<bool> ExistsAsync(Guid naudotojasId, Guid autoriusId);
        Task<int> GetFollowerCountAsync(Guid autoriusId);
    }
}
