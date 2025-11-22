using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Irasai;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class IrasasService : IIrasasService
    {
        private readonly IIrasasRepository _irasasRepository;
        private readonly IKnygaRepository _knygaRepository;
        private readonly ISekimasRepository _sekimasRepository;
        private readonly IMapper _mapper;

        public IrasasService(
            IIrasasRepository irasasRepository,
            IKnygaRepository knygaRepository,
            ISekimasRepository sekimasRepository,
            IMapper mapper)
        {
            _irasasRepository = irasasRepository;
            _knygaRepository = knygaRepository;
            _sekimasRepository = sekimasRepository;
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
            // Get user's read books
            var perskaitytos = await _irasasRepository.GetByNaudotojasIdAndTipasAsync(naudotojasId, BookshelfTypes.skaityta);
            var perskaitytosKnygosIds = perskaitytos.Select(i => i.KnygaId).ToHashSet();

            // Get user's all books in bookshelf
            var visos = await _irasasRepository.GetByNaudotojasIdAsync(naudotojasId);
            var visosKnygosIds = visos.Select(i => i.KnygaId).ToHashSet();

            // Get user's followed authors
            var sekimai = await _sekimasRepository.GetByNaudotojasIdAsync(naudotojasId);
            var sekimuAutoriaiIds = sekimai.Select(s => s.AutoriusId).ToHashSet();

            // Get genres from read books
            var zanraiIds = new HashSet<Guid>();
            foreach (var irasas in perskaitytos)
            {
                if (irasas.Knyga?.KnygaZanrai != null)
                {
                    foreach (var kz in irasas.Knyga.KnygaZanrai)
                    {
                        zanraiIds.Add(kz.ZanrasId);
                    }
                }
            }

            // Get popular books
            var populiariosKnygos = await _knygaRepository.GetPopularBooksAsync(50);

            // Filter and score recommendations
            var rekomendacijos = populiariosKnygos
                .Where(k => !visosKnygosIds.Contains(k.Id))
                .Select(k => new
                {
                    Knyga = k,
                    Score = CalculateRecommendationScore(k, zanraiIds, sekimuAutoriaiIds)
                })
                .OrderByDescending(x => x.Score)
                .Take(10)
                .Select(x => x.Knyga)
                .ToList();

            var result = new List<KnygaListDto>();
            foreach (var knyga in rekomendacijos)
            {
                var dto = _mapper.Map<KnygaListDto>(knyga);
                dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                result.Add(dto);
            }

            return result;
        }

        private static int CalculateRecommendationScore(Knyga knyga, HashSet<Guid> zanraiIds, HashSet<Guid> autoriaiIds)
        {
            int score = 0;

            // Score for matching genres
            if (knyga.KnygaZanrai != null)
            {
                score += knyga.KnygaZanrai.Count(kz => zanraiIds.Contains(kz.ZanrasId)) * 2;
            }

            // Score for followed author
            if (autoriaiIds.Contains(knyga.AutoriusId))
            {
                score += 5;
            }

            // Score for bestseller
            if (knyga.bestseleris)
            {
                score += 1;
            }

            return score;
        }
    }
}
