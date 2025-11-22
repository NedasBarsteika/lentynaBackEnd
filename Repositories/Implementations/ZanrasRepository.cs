using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class ZanrasRepository : IZanrasRepository
    {
        private readonly ApplicationDbContext _context;

        public ZanrasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Zanras?> GetByIdAsync(Guid id)
        {
            return await _context.Zanrai.FindAsync(id);
        }

        public async Task<IEnumerable<Zanras>> GetAllAsync()
        {
            return await _context.Zanrai
                .OrderBy(z => z.pavadinimas)
                .ToListAsync();
        }

        public async Task<Zanras> AddAsync(Zanras zanras)
        {
            zanras.Id = Guid.NewGuid();
            await _context.Zanrai.AddAsync(zanras);
            await _context.SaveChangesAsync();
            return zanras;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var zanras = await _context.Zanrai.FindAsync(id);
            if (zanras == null) return false;

            _context.Zanrai.Remove(zanras);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string pavadinimas)
        {
            return await _context.Zanrai.AnyAsync(z => z.pavadinimas == pavadinimas);
        }
    }
}
