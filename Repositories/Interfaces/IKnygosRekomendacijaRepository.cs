using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Repositories.Interfaces
{
    public interface IKnygosRekomendacijaRepository
    {
        Task<Knygos_rekomendacija> AddAsync(Guid naudotojasId);
        Task<Knygos_rekomendacija?> GetLatestByNaudotojasIdAsync(Guid naudotojasId);
        Task UpdateAsync(Knygos_rekomendacija rekomendacija);
    }
}
