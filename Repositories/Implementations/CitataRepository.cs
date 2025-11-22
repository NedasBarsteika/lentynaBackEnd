using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class CitataRepository : ICitataRepository
    {
        private readonly ApplicationDbContext _context;

        public CitataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Citata?> GetByIdAsync(Guid id)
        {
            return await _context.Citatos
                .Include(c => c.Autorius)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Citata>> GetByAutoriusIdAsync(Guid autoriusId)
        {
            return await _context.Citatos
                .Where(c => c.AutoriusId == autoriusId)
                .OrderByDescending(c => c.citatos_data)
                .ToListAsync();
        }

        public async Task<Citata> AddAsync(Citata citata)
        {
            citata.Id = Guid.NewGuid();
            await _context.Citatos.AddAsync(citata);
            await _context.SaveChangesAsync();
            return citata;
        }

        public async Task UpdateAsync(Citata citata)
        {
            _context.Citatos.Update(citata);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var citata = await _context.Citatos.FindAsync(id);
            if (citata == null) return false;

            _context.Citatos.Remove(citata);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
