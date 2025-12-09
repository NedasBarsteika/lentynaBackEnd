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
                .Include(k => k.Zanras)
                .Include(k => k.DI_Komentarai.OrderByDescending(d => d.sugeneravimo_data).Take(1))
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<(IEnumerable<Knyga> Items, int TotalCount)> GetAllAsync(KnygaFilterDto filter)
        {
            var query = _context.Knygos
                .Include(k => k.Autorius)
                .Include(k => k.Zanras)
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
                query = query.Where(k => k.ZanrasId == filter.zanrasId.Value);
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

        public async Task<IEnumerable<Knyga>> GetPopularBooksAsync(int count)
        {
            return await _context.Knygos
                .Include(k => k.Autorius)
                .Include(k => k.Zanras)
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

        public async Task<IEnumerable<Knyga>> GetBooksForAdvancedSearchAsync(List<Guid>? zanruIds, List<Guid>? nuotaikuIds)
        {
            var query = _context.Knygos
                .Include(k => k.Autorius)
                .Include(k => k.Zanras)
                .AsQueryable();

            // Filter by genres if provided
            if (zanruIds != null && zanruIds.Count > 0)
            {
                query = query.Where(k => zanruIds.Contains(k.ZanrasId));
            }

            // Filter by moods through Zanras relationship
            if (nuotaikuIds != null && nuotaikuIds.Count > 0)
            {
                // Get ZanrasIds from the NuotaikosZanrai join table
                var zanruIdsFromNuotaikos = await _context.NuotaikosZanrai
                    .Where(nz => nuotaikuIds.Contains(nz.NuotaikaId))
                    .Select(nz => nz.ZanrasId)
                    .Distinct()
                    .ToListAsync();

                // If we also have genre filter, intersect the results
                if (zanruIds != null && zanruIds.Count > 0)
                {
                    var intersectedZanruIds = zanruIds.Intersect(zanruIdsFromNuotaikos).ToList();
                    query = query.Where(k => intersectedZanruIds.Contains(k.ZanrasId));
                }
                else
                {
                    query = query.Where(k => zanruIdsFromNuotaikos.Contains(k.ZanrasId));
                }
            }

            return await query.ToListAsync();
        }
    }
}
