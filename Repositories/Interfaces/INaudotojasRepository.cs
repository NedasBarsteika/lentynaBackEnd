using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface INaudotojasRepository
    {
        Task<Naudotojas?> GetByIdAsync(Guid id);
        Task<Naudotojas?> GetByEmailAsync(string email);
        Task<Naudotojas?> GetBySlapyvardisAsync(string slapyvardis);
        Task<IEnumerable<Naudotojas>> GetAllAsync();
        Task<Naudotojas> AddAsync(Naudotojas naudotojas);
        Task UpdateAsync(Naudotojas naudotojas);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsBySlapyvardisAsync(string slapyvardis);
    }
}
