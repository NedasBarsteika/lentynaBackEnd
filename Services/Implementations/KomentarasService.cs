using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Komentarai;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class KomentarasService : IKomentarasService
    {
        private readonly IKomentarasRepository _komentarasRepository;
        private readonly IKnygaRepository _knygaRepository;
        private readonly ITemaRepository _temaRepository;
        private readonly IMapper _mapper;

        public KomentarasService(
            IKomentarasRepository komentarasRepository,
            IKnygaRepository knygaRepository,
            ITemaRepository temaRepository,
            IMapper mapper)
        {
            _komentarasRepository = komentarasRepository;
            _knygaRepository = knygaRepository;
            _temaRepository = temaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<KomentarasDto>> GetByKnygaIdAsync(Guid knygaId)
        {
            var komentarai = await _komentarasRepository.GetByKnygaIdAsync(knygaId);
            return _mapper.Map<IEnumerable<KomentarasDto>>(komentarai);
        }

        public async Task<IEnumerable<KomentarasDto>> GetByTemaIdAsync(Guid temaId)
        {
            var komentarai = await _komentarasRepository.GetByTemaIdAsync(temaId);
            return _mapper.Map<IEnumerable<KomentarasDto>>(komentarai);
        }

        public async Task<(Result Result, KomentarasDto? Komentaras)> CreateAsync(Guid naudotojasId, CreateKomentarasDto dto)
        {
            if (dto.KnygaId.HasValue)
            {
                var knyga = await _knygaRepository.GetByIdAsync(dto.KnygaId.Value);
                if (knyga == null)
                {
                    return (Result.Failure(Constants.KnygaNerastas), null);
                }
            }

            if (dto.TemaId.HasValue)
            {
                var tema = await _temaRepository.GetByIdAsync(dto.TemaId.Value);
                if (tema == null)
                {
                    return (Result.Failure(Constants.TemaNerastas), null);
                }
            }

            var komentaras = new Komentaras
            {
                komentaro_tekstas = dto.komentaro_tekstas,
                vertinimas = dto.vertinimas,
                NaudotojasId = naudotojasId,
                KnygaId = dto.KnygaId,
                TemaId = dto.TemaId
            };

            await _komentarasRepository.AddAsync(komentaras);

            var result = await _komentarasRepository.GetByIdAsync(komentaras.Id);
            return (Result.Success(), _mapper.Map<KomentarasDto>(result));
        }

        public async Task<(Result Result, KomentarasDto? Komentaras)> UpdateAsync(Guid id, Guid naudotojasId, UpdateKomentarasDto dto)
        {
            var komentaras = await _komentarasRepository.GetByIdAsync(id);

            if (komentaras == null)
            {
                return (Result.Failure(Constants.KomentarasNerastas), null);
            }

            if (komentaras.NaudotojasId != naudotojasId)
            {
                return (Result.Failure(Constants.NeturitePrieigos), null);
            }

            if (!string.IsNullOrEmpty(dto.komentaro_tekstas))
            {
                komentaras.komentaro_tekstas = dto.komentaro_tekstas;
            }

            if (dto.vertinimas.HasValue)
            {
                komentaras.vertinimas = dto.vertinimas.Value;
            }

            await _komentarasRepository.UpdateAsync(komentaras);

            var result = await _komentarasRepository.GetByIdAsync(id);
            return (Result.Success(), _mapper.Map<KomentarasDto>(result));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid naudotojasId, bool isAdmin)
        {
            var komentaras = await _komentarasRepository.GetByIdAsync(id);

            if (komentaras == null)
            {
                return Result.Failure(Constants.KomentarasNerastas);
            }

            if (komentaras.NaudotojasId != naudotojasId && !isAdmin)
            {
                return Result.Failure(Constants.NeturitePrieigos);
            }

            await _komentarasRepository.DeleteAsync(id);

            return Result.Success();
        }
    }
}
