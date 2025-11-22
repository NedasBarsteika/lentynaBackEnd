using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IKnygaRepository
    {
        Task<Knyga?> GetByIdAsync(Guid id);
        Task<Knyga?> GetByIdWithDetailsAsync(Guid id);
        Task<(IEnumerable<Knyga> Items, int TotalCount)> GetAllAsync(KnygaFilterDto filter);
        Task<Knyga> AddAsync(Knyga knyga);
        Task UpdateAsync(Knyga knyga);
        Task<bool> DeleteAsync(Guid id);
        Task AddZanraiAsync(Guid knygaId, List<Guid> zanraiIds);
        Task AddNuotaikosAsync(Guid knygaId, List<Guid> nuotaikosIds);
        Task RemoveZanraiAsync(Guid knygaId);
        Task RemoveNuotaikosAsync(Guid knygaId);
        Task<IEnumerable<Knyga>> GetPopularBooksAsync(int count);
        Task<double> GetAverageRatingAsync(Guid knygaId);
        Task<int> GetReviewCountAsync(Guid knygaId);
    }
}
