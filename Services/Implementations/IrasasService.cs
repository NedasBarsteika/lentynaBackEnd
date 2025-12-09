using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Irasai;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;
using System.Collections.Concurrent;

namespace lentynaBackEnd.Services.Implementations
{
    public class IrasasService : IIrasasService
    {
        private static readonly ConcurrentDictionary<Guid, CachedRecommendations> _recommendationCache = new();

        private readonly IIrasasRepository _irasasRepository;
        private readonly IKnygaRepository _knygaRepository;
        private readonly ISekimasRepository _sekimasRepository;
        private readonly IKnygosRekomendacijaRepository _knygosRekomendacijaRepository;
        private readonly IOpenAIService _openAIService;
        private readonly INaudotojasRepository _naudotojasRepository;
        private readonly ILogger<IrasasService> _logger;
        private readonly IMapper _mapper;

        public IrasasService(
            IIrasasRepository irasasRepository,
            IKnygaRepository knygaRepository,
            ISekimasRepository sekimasRepository,
            IKnygosRekomendacijaRepository knygosRekomendacijaRepository,
            IOpenAIService openAIService,
            INaudotojasRepository naudotojasRepository,
            ILogger<IrasasService> logger,
            IMapper mapper)
        {
            _irasasRepository = irasasRepository;
            _knygaRepository = knygaRepository;
            _sekimasRepository = sekimasRepository;
            _knygosRekomendacijaRepository = knygosRekomendacijaRepository;
            _openAIService = openAIService;
            _naudotojasRepository = naudotojasRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IrasasDto>> GetByNaudotojasIdAsync(Guid naudotojasId)
        {
            var irasai = await _irasasRepository.GetByNaudotojasIdAsync(naudotojasId);
            return _mapper.Map<IEnumerable<IrasasDto>>(irasai);
        }

        public async Task<IEnumerable<IrasasDto>> GetByNaudotojasIdAndTipasAsync(Guid naudotojasId, BookshelfTypes tipas)
        {
            var irasai = await _irasasRepository.GetByNaudotojasIdAndTipasAsync(naudotojasId, tipas);
            return _mapper.Map<IEnumerable<IrasasDto>>(irasai);
        }

        public async Task<(Result Result, IrasasDto? Irasas)> CreateAsync(Guid naudotojasId, CreateIrasasDto dto)
        {
            var naudotojas = await _naudotojasRepository.GetByIdAsync(naudotojasId);
            if (naudotojas == null)
            {
                return (Result.Failure(Constants.NaudotojasNerastas), null);
            }

            var knyga = await _knygaRepository.GetByIdAsync(dto.KnygaId);
            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), null);
            }

            if (await _irasasRepository.ExistsAsync(naudotojasId, dto.KnygaId))
            {
                return (Result.Failure(Constants.KnygaJauSarase), null);
            }

            var irasas = new Irasas
            {
                tipas = dto.tipas,
                NaudotojasId = naudotojasId,
                KnygaId = dto.KnygaId
            };

            await _irasasRepository.AddAsync(irasas);

            var result = await _irasasRepository.GetByIdAsync(irasas.Id);
            return (Result.Success(), _mapper.Map<IrasasDto>(result));
        }

        public async Task<(Result Result, IrasasDto? Irasas)> UpdateAsync(Guid id, Guid naudotojasId, UpdateIrasasDto dto)
        {
            var irasas = await _irasasRepository.GetByIdAsync(id);

            if (irasas == null)
            {
                return (Result.Failure(Constants.IrasasNerastas), null);
            }

            if (irasas.NaudotojasId != naudotojasId)
            {
                return (Result.Failure(Constants.NeturitePrieigos), null);
            }

            irasas.tipas = dto.tipas;
            await _irasasRepository.UpdateAsync(irasas);

            var result = await _irasasRepository.GetByIdAsync(id);
            return (Result.Success(), _mapper.Map<IrasasDto>(result));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid naudotojasId)
        {
            var irasas = await _irasasRepository.GetByIdAsync(id);

            if (irasas == null)
            {
                return Result.Failure(Constants.IrasasNerastas);
            }

            if (irasas.NaudotojasId != naudotojasId)
            {
                return Result.Failure(Constants.NeturitePrieigos);
            }

            await _irasasRepository.DeleteAsync(id);

            return Result.Success();
        }

        public async Task<IEnumerable<KnygaListDto>> GetRekomendacijosAsync(Guid naudotojasId)
        {
            var naudotojas = await _naudotojasRepository.GetByIdAsync(naudotojasId);
            if (naudotojas == null)
            {
                return Enumerable.Empty<KnygaListDto>();
            }

            var latestDbRecommendation = await _knygosRekomendacijaRepository.GetLatestByNaudotojasIdAsync(naudotojasId);

            // Reuse cache if DB window is valid and cache is present
            if (latestDbRecommendation != null &&
                latestDbRecommendation.rekomendacijos_pabaiga.HasValue &&
                DateTime.UtcNow <= latestDbRecommendation.rekomendacijos_pabaiga.Value &&
                _recommendationCache.TryGetValue(naudotojasId, out var cached) &&
                cached.Items.Count > 0)
            {
                _logger.LogInformation("Returning cached recommendations for user {UserId}", naudotojasId);
                return cached.Items;
            }

            var perskaitytos = (await _irasasRepository.GetByNaudotojasIdAndTipasAsync(naudotojasId, BookshelfTypes.skaityta)).ToList();
            if (perskaitytos.Count == 0)
            {
                return Enumerable.Empty<KnygaListDto>();
            }

            var visos = (await _irasasRepository.GetByNaudotojasIdAsync(naudotojasId)).ToList();
            var visosKnygosIds = visos.Select(i => i.KnygaId).ToHashSet();

            var kandidatai = (await _knygaRepository.GetPopularBooksAsync(50))
                .Where(k => !visosKnygosIds.Contains(k.Id))
                .ToList();

            if (kandidatai.Count == 0)
            {
                return Enumerable.Empty<KnygaListDto>();
            }

            var rekomendacijuIds = await _openAIService.Generuoti_Rekomendaciju_sarasaAsync(
                perskaitytos,
                kandidatai,
                visosKnygosIds);

            var topIds = rekomendacijuIds.Take(5).ToList();
            if (topIds.Count == 0)
            {
                _logger.LogWarning("AI nerekomendavo knygu naudotojui {UserId}; grąžiname tuščią sąrašą", naudotojasId);
                return Enumerable.Empty<KnygaListDto>();
            }

            var result = new List<KnygaListDto>();

            foreach (var id in topIds)
            {
                var knyga = kandidatai.FirstOrDefault(k => k.Id == id);
                if (knyga == null) continue;

                var dto = _mapper.Map<KnygaListDto>(knyga);
                dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                result.Add(dto);
            }

            // Persist recommendation window (7 days)
            if (latestDbRecommendation == null || !latestDbRecommendation.rekomendacijos_pabaiga.HasValue || DateTime.UtcNow > latestDbRecommendation.rekomendacijos_pabaiga.Value)
            {
                await _knygosRekomendacijaRepository.AddAsync(naudotojasId);
            }
            else
            {
                latestDbRecommendation.rekomendacijos_pradzia = DateTime.UtcNow;
                latestDbRecommendation.rekomendacijos_pabaiga = DateTime.UtcNow.AddDays(7);
                await _knygosRekomendacijaRepository.UpdateAsync(latestDbRecommendation);
            }

            // Store in cache for up to 7 days
            _recommendationCache[naudotojasId] = new CachedRecommendations
            {
                GeneratedAt = DateTime.UtcNow,
                Items = result
            };

            return result;
        }

        private class CachedRecommendations
        {
            public DateTime GeneratedAt { get; set; }
            public List<KnygaListDto> Items { get; set; } = new();
        }
    }
}
