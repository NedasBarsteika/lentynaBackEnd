using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class BalsavimasRepository : IBalsavimasRepository
    {
        private readonly ApplicationDbContext _context;

        public BalsavimasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Balsavimas?> GetByIdAsync(Guid id)
        {
            return await _context.Balsavimai.FindAsync(id);
        }

        public async Task<Balsavimas?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Balsavimai
                .Include(b => b.IsrinktaKnyga)
                    .ThenInclude(k => k!.Autorius)
                .Include(b => b.Balsai)
                    .ThenInclude(ba => ba.Knyga)
                    .ThenInclude(k => k!.Autorius)
                .Include(b => b.BalsavimoKnygos)
                    .ThenInclude(bk => bk.Knyga)
                    .ThenInclude(k => k.Autorius)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Balsavimas?> GetCurrentAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Balsavimai
                .Include(b => b.Balsai)
                    .ThenInclude(ba => ba.Knyga)
                    .ThenInclude(k => k!.Autorius)
                .Include(b => b.BalsavimoKnygos)
                    .ThenInclude(bk => bk.Knyga)
                    .ThenInclude(k => k.Autorius)
                .OrderByDescending(b => b.balsavimo_pabaiga)
                // .Where(b => b.balsavimo_pradzia <= now && b.balsavimo_pabaiga >= now)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Balsavimas>> GetAllAsync()
        {
            return await _context.Balsavimai
                .Include(b => b.IsrinktaKnyga)
                .OrderByDescending(b => b.balsavimo_pradzia)
                .ToListAsync();
        }

        public async Task<Balsavimas> AddAsync(Balsavimas balsavimas)
        {
            balsavimas.Id = Guid.NewGuid();
            await _context.Balsavimai.AddAsync(balsavimas);
            await _context.SaveChangesAsync();
            return balsavimas;
        }

        public async Task UpdateAsync(Balsavimas balsavimas)
        {
            _context.Balsavimai.Update(balsavimas);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var balsavimas = await _context.Balsavimai.FindAsync(id);
            if (balsavimas == null) return false;

            _context.Balsavimai.Remove(balsavimas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Balsas> AddBalsasAsync(Balsas balsas)
        {
            balsas.Id = Guid.NewGuid();
            balsas.pateikimo_data = DateTime.UtcNow;
            await _context.Balsai.AddAsync(balsas);
            await _context.SaveChangesAsync();
            return balsas;
        }

        public async Task<bool> DeleteBalsasAsync(Guid balsasId)
        {
            var balsas = await _context.Balsai.FindAsync(balsasId);
            if (balsas == null) return false;

            _context.Balsai.Remove(balsas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasVotedAsync(Guid balsavimasId, Guid naudotojasId)
        {
            return await _context.Balsai
                .AnyAsync(b => b.BalsavimasId == balsavimasId && b.NaudotojasId == naudotojasId);
        }

        public async Task<Balsas?> GetBalsasByIdAsync(Guid balsasId)
        {
            return await _context.Balsai.FindAsync(balsasId);
        }

        public async Task<Dictionary<Guid, int>> GetVoteCountsAsync(Guid balsavimasId)
        {
            return await _context.Balsai
                .Where(b => b.BalsavimasId == balsavimasId)
                .GroupBy(b => b.KnygaId)
                .Select(g => new { KnygaId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.KnygaId, x => x.Count);
        }

        public async Task AddBalsavimoKnygaAsync(BalsavimoKnyga balsavimoKnyga)
        {
            await _context.BalsavimoKnygos.AddAsync(balsavimoKnyga);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsKnygaNominuotaAsync(Guid balsavimasId, Guid knygaId)
        {
            return await _context.BalsavimoKnygos
                .AnyAsync(bk => bk.BalsavimasId == balsavimasId && bk.KnygaId == knygaId);
        }
    }
}
