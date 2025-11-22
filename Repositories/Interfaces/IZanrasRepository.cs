using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IZanrasRepository
    {
        Task<Zanras?> GetByIdAsync(Guid id);
        Task<IEnumerable<Zanras>> GetAllAsync();
        Task<Zanras> AddAsync(Zanras zanras);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByNameAsync(string pavadinimas);
    }
}
