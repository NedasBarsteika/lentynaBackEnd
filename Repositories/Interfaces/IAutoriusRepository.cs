using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IAutoriusRepository
    {
        Task<Autorius?> GetByIdAsync(Guid id);
        Task<Autorius?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Autorius>> GetAllAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Autorius> AddAsync(Autorius autorius);
        Task UpdateAsync(Autorius autorius);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Knyga>> GetKnygosAsync(Guid autoriusId);
        Task<IEnumerable<Citata>> GetCitatosAsync(Guid autoriusId);
        Task UpdateKnyguSkaicius(Guid autoriusId);
    }
}
