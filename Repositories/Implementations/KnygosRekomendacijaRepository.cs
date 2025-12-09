using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class KnygosRekomendacijaRepository : IKnygosRekomendacijaRepository
    {
        private readonly ApplicationDbContext _context;

        public KnygosRekomendacijaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Knygos_rekomendacija> AddAsync(Guid naudotojasId)
        {
            var rekomendacija = new Knygos_rekomendacija
            {
                Id = Guid.NewGuid(),
                NaudotojasId = naudotojasId,
                rekomendacijos_pradzia = DateTime.UtcNow,
                rekomendacijos_pabaiga = DateTime.UtcNow.AddDays(7)
            };

            await _context.Knygos_rekomendacijos.AddAsync(rekomendacija);
            await _context.SaveChangesAsync();

            return rekomendacija;
        }

        public async Task<Knygos_rekomendacija?> GetLatestByNaudotojasIdAsync(Guid naudotojasId)
        {
            return await _context.Knygos_rekomendacijos
                .Where(r => r.NaudotojasId == naudotojasId)
                .OrderByDescending(r => r.rekomendacijos_pradzia)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Knygos_rekomendacija rekomendacija)
        {
            _context.Knygos_rekomendacijos.Update(rekomendacija);
            await _context.SaveChangesAsync();
        }
    }
}
