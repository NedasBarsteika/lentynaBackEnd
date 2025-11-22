using lentynaBackEnd.Data;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Repositories.Implementations
{
    public class KnygaRepository : IKnygaRepository
    {
        private readonly ApplicationDbContext _context;

        public KnygaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Knyga?> GetByIdAsync(Guid id)
        {
            return await _context.Knygos.FindAsync(id);
        }

        public async Task<Knyga?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Knygos
                .Include(k => k.Autorius)
                .Include(k => k.KnygaZanrai)
                    .ThenInclude(kz => kz.Zanras)
                .Include(k => k.KnygaNuotaikos)
                    .ThenInclude(kn => kn.Nuotaika)
                .Include(k => k.DI_Komentarai.OrderByDescending(d => d.sugeneravimo_data).Take(1))
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<(IEnumerable<Knyga> Items, int TotalCount)> GetAllAsync(KnygaFilterDto filter)
        {
            var query = _context.Knygos
                .Include(k => k.Autorius)
                .Include(k => k.KnygaZanrai)
                    .ThenInclude(kz => kz.Zanras)
                .Include(k => k.KnygaNuotaikos)
                    .ThenInclude(kn => kn.Nuotaika)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrEmpty(filter.paieska))
            {
                var search = filter.paieska.ToLower();
                query = query.Where(k =>
                    k.knygos_pavadinimas.ToLower().Contains(search) ||
                    (k.aprasymas != null && k.aprasymas.ToLower().Contains(search)) ||
                    (k.Autorius != null && (k.Autorius.vardas.ToLower().Contains(search) || k.Autorius.pavarde.ToLower().Contains(search))));
            }

            // Genre filter
            if (filter.zanrasId.HasValue)
            {
                query = query.Where(k => k.KnygaZanrai.Any(kz => kz.ZanrasId == filter.zanrasId.Value));
            }

            // Mood filter
            if (filter.nuotaikaId.HasValue)
            {
                query = query.Where(k => k.KnygaNuotaikos.Any(kn => kn.NuotaikaId == filter.nuotaikaId.Value));
            }

            // Author filter
            if (filter.autoriusId.HasValue)
            {
                query = query.Where(k => k.AutoriusId == filter.autoriusId.Value);
            }

            // Bestseller filter
            if (filter.bestseleris.HasValue)
            {
                query = query.Where(k => k.bestseleris == filter.bestseleris.Value);
            }

            var totalCount = await query.CountAsync();

            // Sorting
            query = filter.sortBy?.ToLower() switch
            {
                "leidimo_metai" => filter.descending
                    ? query.OrderByDescending(k => k.leidimo_metai)
                    : query.OrderBy(k => k.leidimo_metai),
                "pavadinimas" => filter.descending
                    ? query.OrderByDescending(k => k.knygos_pavadinimas)
                    : query.OrderBy(k => k.knygos_pavadinimas),
                _ => query.OrderBy(k => k.knygos_pavadinimas)
            };

            var items = await query
                .Skip((filter.page - 1) * filter.pageSize)
                .Take(filter.pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Knyga> AddAsync(Knyga knyga)
        {
            knyga.Id = Guid.NewGuid();
            await _context.Knygos.AddAsync(knyga);
            await _context.SaveChangesAsync();
            return knyga;
        }

        public async Task UpdateAsync(Knyga knyga)
        {
            _context.Knygos.Update(knyga);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var knyga = await _context.Knygos.FindAsync(id);
            if (knyga == null) return false;

            _context.Knygos.Remove(knyga);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddZanraiAsync(Guid knygaId, List<Guid> zanraiIds)
        {
            var knygaZanrai = zanraiIds.Select(zId => new KnygaZanras
            {
                KnygaId = knygaId,
                ZanrasId = zId
            });
            await _context.KnygaZanrai.AddRangeAsync(knygaZanrai);
            await _context.SaveChangesAsync();
        }

        public async Task AddNuotaikosAsync(Guid knygaId, List<Guid> nuotaikosIds)
        {
            var knygaNuotaikos = nuotaikosIds.Select(nId => new KnygaNuotaika
            {
                KnygaId = knygaId,
                NuotaikaId = nId
            });
            await _context.KnygaNuotaikos.AddRangeAsync(knygaNuotaikos);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveZanraiAsync(Guid knygaId)
        {
            var knygaZanrai = await _context.KnygaZanrai
                .Where(kz => kz.KnygaId == knygaId)
                .ToListAsync();
            _context.KnygaZanrai.RemoveRange(knygaZanrai);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveNuotaikosAsync(Guid knygaId)
        {
            var knygaNuotaikos = await _context.KnygaNuotaikos
                .Where(kn => kn.KnygaId == knygaId)
                .ToListAsync();
            _context.KnygaNuotaikos.RemoveRange(knygaNuotaikos);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Knyga>> GetPopularBooksAsync(int count)
        {
            return await _context.Knygos
                .Include(k => k.Autorius)
                .Include(k => k.Komentarai)
                .OrderByDescending(k => k.Komentarai.Count)
                .ThenByDescending(k => k.Komentarai.Average(ko => (double?)ko.vertinimas) ?? 0)
                .Take(count)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(Guid knygaId)
        {
            var ratings = await _context.Komentarai
                .Where(k => k.KnygaId == knygaId)
                .Select(k => k.vertinimas)
                .ToListAsync();

            return ratings.Count > 0 ? ratings.Average() : 0;
        }

        public async Task<int> GetReviewCountAsync(Guid knygaId)
        {
            return await _context.Komentarai
                .CountAsync(k => k.KnygaId == knygaId);
        }
    }
}
