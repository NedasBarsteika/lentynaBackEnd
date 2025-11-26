using Microsoft.EntityFrameworkCore;
using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class DIKomentarasRepository : IDIKomentarasRepository
    {
        private readonly ApplicationDbContext _context;

        public DIKomentarasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dirbtinio_intelekto_komentaras?> GetByKnygaIdAsync(Guid knygaId)
        {
            return await _context.DI_Komentarai
                .Where(d => d.KnygaId == knygaId)
                .OrderByDescending(d => d.sugeneravimo_data)
                .FirstOrDefaultAsync();
        }

        public async Task<Dirbtinio_intelekto_komentaras> AddAsync(Dirbtinio_intelekto_komentaras diKomentaras)
        {
            diKomentaras.Id = Guid.NewGuid();
            diKomentaras.sugeneravimo_data = DateTime.UtcNow;

            await _context.DI_Komentarai.AddAsync(diKomentaras);
            await _context.SaveChangesAsync();

            return diKomentaras;
        }

        public async Task UpdateAsync(Dirbtinio_intelekto_komentaras diKomentaras)
        {
            diKomentaras.sugeneravimo_data = DateTime.UtcNow;

            _context.DI_Komentarai.Update(diKomentaras);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var diKomentaras = await _context.DI_Komentarai.FindAsync(id);
            if (diKomentaras == null)
                return false;

            _context.DI_Komentarai.Remove(diKomentaras);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> NeedsRegenerationAsync(Guid knygaId)
        {
            var latestComment = await GetByKnygaIdAsync(knygaId);

            // If no comment exists, needs generation
            if (latestComment == null)
                return true;

            // Check if comment is older than 7 days
            var weekAgo = DateTime.UtcNow.AddDays(-7);
            return latestComment.sugeneravimo_data < weekAgo;
        }
    }
}
