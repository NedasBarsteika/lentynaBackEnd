using AutoMapper;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Common;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class KnygaService : IKnygaService
    {
        private readonly IKnygaRepository _knygaRepository;
        private readonly IAutoriusRepository _autoriusRepository;
        private readonly IMapper _mapper;

        public KnygaService(
            IKnygaRepository knygaRepository,
            IAutoriusRepository autoriusRepository,
            IMapper mapper)
        {
            _knygaRepository = knygaRepository;
            _autoriusRepository = autoriusRepository;
            _mapper = mapper;
        }

        public async Task<(Result Result, KnygaDetailDto? Knyga)> GetByIdAsync(Guid id)
        {
            var knyga = await _knygaRepository.GetByIdWithDetailsAsync(id);

            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), null);
            }

            var dto = _mapper.Map<KnygaDetailDto>(knyga);
            dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(id);
            dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(id);

            return (Result.Success(), dto);
        }

        public async Task<PaginatedResultDto<KnygaListDto>> GetAllAsync(KnygaFilterDto filter)
        {
            var (items, totalCount) = await _knygaRepository.GetAllAsync(filter);

            var dtos = new List<KnygaListDto>();
            foreach (var knyga in items)
            {
                var dto = _mapper.Map<KnygaListDto>(knyga);
                dto.vidutinis_vertinimas = await _knygaRepository.GetAverageRatingAsync(knyga.Id);
                dto.komentaru_skaicius = await _knygaRepository.GetReviewCountAsync(knyga.Id);
                dtos.Add(dto);
            }

            return new PaginatedResultDto<KnygaListDto>(dtos, filter.page, filter.pageSize, totalCount);
        }

        public async Task<(Result Result, KnygaDetailDto? Knyga)> CreateAsync(CreateKnygaDto dto)
        {
            var autorius = await _autoriusRepository.GetByIdAsync(dto.AutoriusId);
            if (autorius == null)
            {
                return (Result.Failure(Constants.AutoriusNerastas), null);
            }

            var knyga = new Knyga
            {
                knygos_pavadinimas = dto.knygos_pavadinimas,
                leidimo_metai = dto.leidimo_metai,
                aprasymas = dto.aprasymas,
                psl_skaicius = dto.psl_skaicius,
                ISBN = dto.ISBN,
                virselio_nuotrauka = dto.virselio_nuotrauka,
                raisos = dto.raisos,
                bestseleris = dto.bestseleris,
                AutoriusId = dto.AutoriusId
            };

            await _knygaRepository.AddAsync(knyga);

            if (dto.ZanraiIds.Count > 0)
            {
                await _knygaRepository.AddZanraiAsync(knyga.Id, dto.ZanraiIds);
            }

            if (dto.NuotaikosIds.Count > 0)
            {
                await _knygaRepository.AddNuotaikosAsync(knyga.Id, dto.NuotaikosIds);
            }

            await _autoriusRepository.UpdateKnyguSkaicius(dto.AutoriusId);

            return await GetByIdAsync(knyga.Id);
        }

        public async Task<(Result Result, KnygaDetailDto? Knyga)> UpdateAsync(Guid id, UpdateKnygaDto dto)
        {
            var knyga = await _knygaRepository.GetByIdAsync(id);

            if (knyga == null)
            {
                return (Result.Failure(Constants.KnygaNerastas), null);
            }

            var oldAutoriusId = knyga.AutoriusId;

            if (!string.IsNullOrEmpty(dto.knygos_pavadinimas)) knyga.knygos_pavadinimas = dto.knygos_pavadinimas;
            if (dto.leidimo_metai.HasValue) knyga.leidimo_metai = dto.leidimo_metai;
            if (dto.aprasymas != null) knyga.aprasymas = dto.aprasymas;
            if (dto.psl_skaicius.HasValue) knyga.psl_skaicius = dto.psl_skaicius;
            if (dto.ISBN != null) knyga.ISBN = dto.ISBN;
            if (dto.virselio_nuotrauka != null) knyga.virselio_nuotrauka = dto.virselio_nuotrauka;
            if (dto.raisos != null) knyga.raisos = dto.raisos;
            if (dto.bestseleris.HasValue) knyga.bestseleris = dto.bestseleris.Value;

            if (dto.AutoriusId.HasValue && dto.AutoriusId.Value != knyga.AutoriusId)
            {
                var newAutorius = await _autoriusRepository.GetByIdAsync(dto.AutoriusId.Value);
                if (newAutorius == null)
                {
                    return (Result.Failure(Constants.AutoriusNerastas), null);
                }
                knyga.AutoriusId = dto.AutoriusId.Value;
            }

            await _knygaRepository.UpdateAsync(knyga);

            if (dto.ZanraiIds != null)
            {
                await _knygaRepository.RemoveZanraiAsync(id);
                if (dto.ZanraiIds.Count > 0)
                {
                    await _knygaRepository.AddZanraiAsync(id, dto.ZanraiIds);
                }
            }

            if (dto.NuotaikosIds != null)
            {
                await _knygaRepository.RemoveNuotaikosAsync(id);
                if (dto.NuotaikosIds.Count > 0)
                {
                    await _knygaRepository.AddNuotaikosAsync(id, dto.NuotaikosIds);
                }
            }

            if (dto.AutoriusId.HasValue && dto.AutoriusId.Value != oldAutoriusId)
            {
                await _autoriusRepository.UpdateKnyguSkaicius(oldAutoriusId);
                await _autoriusRepository.UpdateKnyguSkaicius(dto.AutoriusId.Value);
            }

            return await GetByIdAsync(id);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var knyga = await _knygaRepository.GetByIdAsync(id);
            if (knyga == null)
            {
                return Result.Failure(Constants.KnygaNerastas);
            }

            var autoriusId = knyga.AutoriusId;

            var success = await _knygaRepository.DeleteAsync(id);
            if (!success)
            {
                return Result.Failure(Constants.KnygaNerastas);
            }

            await _autoriusRepository.UpdateKnyguSkaicius(autoriusId);

            return Result.Success();
        }
    }
}
