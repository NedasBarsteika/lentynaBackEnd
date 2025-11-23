using lentynaBackEnd.Data;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class IrasasRepository : IIrasasRepository
    {
        private readonly ApplicationDbContext _context;

        public IrasasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Irasas?> GetByIdAsync(Guid id)
        {
            return await _context.Irasai
                .Include(i => i.Knyga)
                    .ThenInclude(k => k!.Autorius)
                .Include(i => i.Knyga)
                    .ThenInclude(k => k!.Zanras)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Irasas>> GetByNaudotojasIdAsync(Guid naudotojasId)
        {
            return await _context.Irasai
                .Where(i => i.NaudotojasId == naudotojasId)
                .Include(i => i.Knyga)
                    .ThenInclude(k => k!.Autorius)
                .Include(i => i.Knyga)
                    .ThenInclude(k => k!.Zanras)
                .OrderByDescending(i => i.sukurimo_data)
                .ToListAsync();
        }

        public async Task<IEnumerable<Irasas>> GetByNaudotojasIdAndTipasAsync(Guid naudotojasId, BookshelfTypes tipas)
        {
            return await _context.Irasai
                .Where(i => i.NaudotojasId == naudotojasId && i.tipas == tipas)
                .Include(i => i.Knyga)
                    .ThenInclude(k => k!.Autorius)
                .Include(i => i.Knyga)
                    .ThenInclude(k => k!.Zanras)
                .OrderByDescending(i => i.sukurimo_data)
                .ToListAsync();
        }

        public async Task<Irasas?> GetByNaudotojasIdAndKnygaIdAsync(Guid naudotojasId, Guid knygaId)
        {
            return await _context.Irasai
                .FirstOrDefaultAsync(i => i.NaudotojasId == naudotojasId && i.KnygaId == knygaId);
        }

        public async Task<Irasas> AddAsync(Irasas irasas)
        {
            irasas.Id = Guid.NewGuid();
            irasas.sukurimo_data = DateTime.UtcNow;
            await _context.Irasai.AddAsync(irasas);
            await _context.SaveChangesAsync();
            return irasas;
        }

        public async Task UpdateAsync(Irasas irasas)
        {
            irasas.redagavimo_data = DateTime.UtcNow;
            _context.Irasai.Update(irasas);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var irasas = await _context.Irasai.FindAsync(id);
            if (irasas == null) return false;

            _context.Irasai.Remove(irasas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid naudotojasId, Guid knygaId)
        {
            return await _context.Irasai
                .AnyAsync(i => i.NaudotojasId == naudotojasId && i.KnygaId == knygaId);
        }
    }
}
