using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class NuotaikaRepository : INuotaikaRepository
    {
        private readonly ApplicationDbContext _context;

        public NuotaikaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Nuotaika?> GetByIdAsync(Guid id)
        {
            return await _context.Nuotaikos.FindAsync(id);
        }

        public async Task<IEnumerable<Nuotaika>> GetAllAsync()
        {
            return await _context.Nuotaikos
                .OrderBy(n => n.pavadinimas)
                .ToListAsync();
        }

        public async Task<Nuotaika> AddAsync(Nuotaika nuotaika)
        {
            nuotaika.Id = Guid.NewGuid();
            await _context.Nuotaikos.AddAsync(nuotaika);
            await _context.SaveChangesAsync();
            return nuotaika;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var nuotaika = await _context.Nuotaikos.FindAsync(id);
            if (nuotaika == null) return false;

            _context.Nuotaikos.Remove(nuotaika);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Nuotaika nuotaika)
        {
            _context.Nuotaikos.Update(nuotaika);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string pavadinimas)
        {
            return await _context.Nuotaikos.AnyAsync(n => n.pavadinimas == pavadinimas);
        }
    }
}
