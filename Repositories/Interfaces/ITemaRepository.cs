using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface ITemaRepository
    {
        Task<Tema?> GetByIdAsync(Guid id);
        Task<Tema?> GetByIdWithDetailsAsync(Guid id);
        Task<(IEnumerable<Tema> Items, int TotalCount)> GetAllAsync(int page, int pageSize);
        Task<Tema> AddAsync(Tema tema);
        Task UpdateAsync(Tema tema);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
