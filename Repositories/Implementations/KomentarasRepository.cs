using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class KomentarasRepository : IKomentarasRepository
    {
        private readonly ApplicationDbContext _context;

        public KomentarasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Komentaras?> GetByIdAsync(Guid id)
        {
            return await _context.Komentarai
                .Include(k => k.Naudotojas)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<IEnumerable<Komentaras>> GetByKnygaIdAsync(Guid knygaId)
        {
            return await _context.Komentarai
                .Where(k => k.KnygaId == knygaId)
                .Include(k => k.Naudotojas)
                .OrderByDescending(k => k.komentaro_data)
                .ToListAsync();
        }

        public async Task<IEnumerable<Komentaras>> GetByTemaIdAsync(Guid temaId)
        {
            return await _context.Komentarai
                .Where(k => k.TemaId == temaId)
                .Include(k => k.Naudotojas)
                .OrderBy(k => k.komentaro_data)
                .ToListAsync();
        }

        public async Task<IEnumerable<Komentaras>> GetByNaudotojasIdAsync(Guid naudotojasId)
        {
            return await _context.Komentarai
                .Where(k => k.NaudotojasId == naudotojasId)
                .OrderByDescending(k => k.komentaro_data)
                .ToListAsync();
        }

        public async Task<Komentaras> AddAsync(Komentaras komentaras)
        {
            komentaras.Id = Guid.NewGuid();
            komentaras.komentaro_data = DateTime.UtcNow;
            await _context.Komentarai.AddAsync(komentaras);
            await _context.SaveChangesAsync();
            return komentaras;
        }

        public async Task UpdateAsync(Komentaras komentaras)
        {
            komentaras.redagavimo_data = DateTime.UtcNow;
            _context.Komentarai.Update(komentaras);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var komentaras = await _context.Komentarai.FindAsync(id);
            if (komentaras == null) return false;

            _context.Komentarai.Remove(komentaras);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
