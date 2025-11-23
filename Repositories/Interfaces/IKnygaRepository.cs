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
        Task<IEnumerable<Knyga>> GetPopularBooksAsync(int count);
        Task<double> GetAverageRatingAsync(Guid knygaId);
        Task<int> GetReviewCountAsync(Guid knygaId);
        Task<IEnumerable<Knyga>> GetBooksForAdvancedSearchAsync(List<Guid>? zanruIds, List<Guid>? nuotaikuIds);
    }
}
