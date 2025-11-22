using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class SekimasRepository : ISekimasRepository
    {
        private readonly ApplicationDbContext _context;

        public SekimasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autoriaus_sekimas>> GetByNaudotojasIdAsync(Guid naudotojasId)
        {
            return await _context.Autoriaus_sekimai
                .Where(s => s.NaudotojasId == naudotojasId)
                .Include(s => s.Autorius)
                .OrderByDescending(s => s.sekimo_pradzia)
                .ToListAsync();
        }

        public async Task<Autoriaus_sekimas?> GetByIdsAsync(Guid naudotojasId, Guid autoriusId)
        {
            return await _context.Autoriaus_sekimai
                .Include(s => s.Autorius)
                .FirstOrDefaultAsync(s => s.NaudotojasId == naudotojasId && s.AutoriusId == autoriusId);
        }

        public async Task<Autoriaus_sekimas> AddAsync(Autoriaus_sekimas sekimas)
        {
            sekimas.sekimo_pradzia = DateTime.UtcNow;
            await _context.Autoriaus_sekimai.AddAsync(sekimas);
            await _context.SaveChangesAsync();
            return sekimas;
        }

        public async Task<bool> DeleteAsync(Guid naudotojasId, Guid autoriusId)
        {
            var sekimas = await _context.Autoriaus_sekimai
                .FirstOrDefaultAsync(s => s.NaudotojasId == naudotojasId && s.AutoriusId == autoriusId);

            if (sekimas == null) return false;

            _context.Autoriaus_sekimai.Remove(sekimas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid naudotojasId, Guid autoriusId)
        {
            return await _context.Autoriaus_sekimai
                .AnyAsync(s => s.NaudotojasId == naudotojasId && s.AutoriusId == autoriusId);
        }

        public async Task<int> GetFollowerCountAsync(Guid autoriusId)
        {
            return await _context.Autoriaus_sekimai
                .CountAsync(s => s.AutoriusId == autoriusId);
        }
    }
}
