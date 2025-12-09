using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface INuotaikaRepository
    {
        Task<Nuotaika?> GetByIdAsync(Guid id);
        Task<IEnumerable<Nuotaika>> GetAllAsync();
        Task<Nuotaika> AddAsync(Nuotaika nuotaika);
        Task<bool> UpdateAsync(Nuotaika nuotaika);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByNameAsync(string pavadinimas);
        Task AddNuotaikosZanrasAsync(NuotaikosZanras nuotaikosZanras);
    }
}
