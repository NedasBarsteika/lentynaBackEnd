using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IIrasasRepository
    {
        Task<Irasas?> GetByIdAsync(Guid id);
        Task<IEnumerable<Irasas>> GetByNaudotojasIdAsync(Guid naudotojasId);
        Task<IEnumerable<Irasas>> GetByNaudotojasIdAndTipasAsync(Guid naudotojasId, BookshelfTypes tipas);
        Task<Irasas?> GetByNaudotojasIdAndKnygaIdAsync(Guid naudotojasId, Guid knygaId);
        Task<Irasas> AddAsync(Irasas irasas);
        Task UpdateAsync(Irasas irasas);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid naudotojasId, Guid knygaId);
    }
}
