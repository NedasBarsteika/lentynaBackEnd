using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class NaudotojasRepository : INaudotojasRepository
    {
        private readonly ApplicationDbContext _context;

        public NaudotojasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Naudotojas?> GetByIdAsync(Guid id)
        {
            return await _context.Naudotojai.FindAsync(id);
        }

        public async Task<Naudotojas?> GetByEmailAsync(string email)
        {
            return await _context.Naudotojai
                .FirstOrDefaultAsync(n => n.el_pastas == email);
        }

        public async Task<Naudotojas?> GetBySlapyvardisAsync(string slapyvardis)
        {
            return await _context.Naudotojai
                .FirstOrDefaultAsync(n => n.slapyvardis == slapyvardis);
        }

        public async Task<IEnumerable<Naudotojas>> GetAllAsync()
        {
            return await _context.Naudotojai.ToListAsync();
        }

        public async Task<Naudotojas> AddAsync(Naudotojas naudotojas)
        {
            naudotojas.Id = Guid.NewGuid();
            naudotojas.sukurimo_data = DateTime.UtcNow;
            await _context.Naudotojai.AddAsync(naudotojas);
            await _context.SaveChangesAsync();
            return naudotojas;
        }

        public async Task UpdateAsync(Naudotojas naudotojas)
        {
            _context.Naudotojai.Update(naudotojas);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var naudotojas = await _context.Naudotojai.FindAsync(id);
            if (naudotojas == null) return false;

            _context.Naudotojai.Remove(naudotojas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Naudotojai.AnyAsync(n => n.el_pastas == email);
        }

        public async Task<bool> ExistsBySlapyvardisAsync(string slapyvardis)
        {
            return await _context.Naudotojai.AnyAsync(n => n.slapyvardis == slapyvardis);
        }
    }
}
