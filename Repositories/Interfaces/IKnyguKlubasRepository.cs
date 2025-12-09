using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IKnyguKlubasRepository
    {
        Task<Balsavimas?> GetByIdAsync(Guid id);
        Task<Balsavimas?> GetByIdWithDetailsAsync(Guid id);
        Task<Balsavimas?> GetCurrentAsync();
        Task<IEnumerable<Balsavimas>> GetAllAsync();
        Task<Balsavimas> AddAsync(Balsavimas balsavimas);
        Task UpdateAsync(Balsavimas balsavimas);
        Task<bool> DeleteAsync(Guid id);
        Task<Balsas> AddBalsasAsync(Balsas balsas);
        Task<bool> DeleteBalsasAsync(Guid balsasId);
        Task<bool> HasVotedAsync(Guid balsavimasId, Guid naudotojasId);
        Task<Balsas?> GetUserVoteAsync(Guid balsavimasId, Guid naudotojasId);
        Task<Balsas?> GetBalsasByIdAsync(Guid balsasId);
        Task<Dictionary<Guid, int>> GetVoteCountsAsync(Guid balsavimasId);
        Task AddBalsavimoKnygaAsync(BalsavimoKnyga balsavimoKnyga);
        Task<bool> IsKnygaNominuotaAsync(Guid balsavimasId, Guid knygaId);
    }
}
