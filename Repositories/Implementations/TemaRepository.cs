using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class TemaRepository : ITemaRepository
    {
        private readonly ApplicationDbContext _context;

        public TemaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tema?> GetByIdAsync(Guid id)
        {
            return await _context.Temos
                .Include(t => t.Naudotojas)
                .FirstOrDefaultAsync(t => t.Id == id && t.istrynimo_data == null);
        }

        public async Task<Tema?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Temos
                .Include(t => t.Naudotojas)
                .Include(t => t.Komentarai.OrderBy(k => k.komentaro_data))
                    .ThenInclude(k => k.Naudotojas)
                .FirstOrDefaultAsync(t => t.Id == id && t.istrynimo_data == null);
        }

        public async Task<(IEnumerable<Tema> Items, int TotalCount)> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Temos
                .Where(t => t.istrynimo_data == null)
                .Include(t => t.Naudotojas)
                .Include(t => t.Komentarai);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.prikabinta)
                .ThenByDescending(t => t.sukurimo_data)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Tema> AddAsync(Tema tema)
        {
            tema.Id = Guid.NewGuid();
            tema.sukurimo_data = DateTime.UtcNow;
            await _context.Temos.AddAsync(tema);
            await _context.SaveChangesAsync();
            return tema;
        }

        public async Task UpdateAsync(Tema tema)
        {
            tema.redagavimo_data = DateTime.UtcNow;
            _context.Temos.Update(tema);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tema = await _context.Temos.FindAsync(id);
            if (tema == null) return false;

            _context.Temos.Remove(tema);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var tema = await _context.Temos.FindAsync(id);
            if (tema == null) return false;

            tema.istrynimo_data = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
