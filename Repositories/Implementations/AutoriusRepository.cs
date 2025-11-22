using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class AutoriusRepository : IAutoriusRepository
    {
        private readonly ApplicationDbContext _context;

        public AutoriusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Autorius?> GetByIdAsync(Guid id)
        {
            return await _context.Autoriai.FindAsync(id);
        }

        public async Task<Autorius?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Autoriai
                .Include(a => a.Knygos)
                    .ThenInclude(k => k.KnygaZanrai)
                    .ThenInclude(kz => kz.Zanras)
                .Include(a => a.Knygos)
                    .ThenInclude(k => k.KnygaNuotaikos)
                    .ThenInclude(kn => kn.Nuotaika)
                .Include(a => a.Citatos)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Autorius>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Autoriai
                .OrderBy(a => a.pavarde)
                .ThenBy(a => a.vardas)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Autoriai.CountAsync();
        }

        public async Task<Autorius> AddAsync(Autorius autorius)
        {
            autorius.Id = Guid.NewGuid();
            await _context.Autoriai.AddAsync(autorius);
            await _context.SaveChangesAsync();
            return autorius;
        }

        public async Task UpdateAsync(Autorius autorius)
        {
            _context.Autoriai.Update(autorius);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var autorius = await _context.Autoriai.FindAsync(id);
            if (autorius == null) return false;

            _context.Autoriai.Remove(autorius);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Knyga>> GetKnygosAsync(Guid autoriusId)
        {
            return await _context.Knygos
                .Where(k => k.AutoriusId == autoriusId)
                .Include(k => k.KnygaZanrai)
                    .ThenInclude(kz => kz.Zanras)
                .Include(k => k.KnygaNuotaikos)
                    .ThenInclude(kn => kn.Nuotaika)
                .ToListAsync();
        }

        public async Task<IEnumerable<Citata>> GetCitatosAsync(Guid autoriusId)
        {
            return await _context.Citatos
                .Where(c => c.AutoriusId == autoriusId)
                .ToListAsync();
        }

        public async Task UpdateKnyguSkaicius(Guid autoriusId)
        {
            var autorius = await _context.Autoriai.FindAsync(autoriusId);
            if (autorius != null)
            {
                autorius.knygu_skaicius = await _context.Knygos
                    .CountAsync(k => k.AutoriusId == autoriusId);
                await _context.SaveChangesAsync();
            }
        }
    }
}
