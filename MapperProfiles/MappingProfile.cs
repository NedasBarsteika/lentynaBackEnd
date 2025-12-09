using AutoMapper;
using lentynaBackEnd.DTOs.Auth;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.DTOs.Irasai;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.DTOs.Komentarai;
using lentynaBackEnd.DTOs.Sekimai;
using lentynaBackEnd.DTOs.Temos;
using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Naudotojas mappings
            CreateMap<Naudotojas, NaudotojasDto>()
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role.ToString()));

            // Autorius mappings
            CreateMap<CreateAutoriusDto, Autorius>();
            CreateMap<Autorius, AutoriusListDto>();
            CreateMap<Autorius, AutoriusDetailDto>()
                .ForMember(dest => dest.knygos, opt => opt.MapFrom(src => src.Knygos))
                .ForMember(dest => dest.citatos, opt => opt.MapFrom(src => src.Citatos));
            CreateMap<Autorius, AutoriusSummaryDto>();

            // Knyga mappings
            CreateMap<Knyga, KnygaListDto>()
                .ForMember(dest => dest.autorius_vardas, opt => opt.MapFrom(src =>
                    src.Autorius != null ? $"{src.Autorius.vardas} {src.Autorius.pavarde}" : ""))
                .ForMember(dest => dest.zanras, opt => opt.MapFrom(src =>
                    src.Zanras != null ? src.Zanras.pavadinimas : ""));

            CreateMap<Knyga, KnygaDetailDto>()
                .ForMember(dest => dest.Zanras, opt => opt.MapFrom(src => src.Zanras));
        

            // Zanras mappings
            CreateMap<Zanras, ZanrasDto>();

            // Nuotaika mappings
            // Zanrai kraunami rankiniu būdu service layer'yje
            CreateMap<Nuotaika, NuotaikaDto>()
                .ForMember(dest => dest.Zanrai, opt => opt.Ignore());

            // Komentaras mappings
            CreateMap<Komentaras, KomentarasDto>()
                .ForMember(dest => dest.naudotojo_slapyvardis, opt => opt.MapFrom(src =>
                    src.Naudotojas != null ? src.Naudotojas.slapyvardis : ""))
                .ForMember(dest => dest.naudotojo_nuotrauka, opt => opt.MapFrom(src =>
                    src.Naudotojas != null ? src.Naudotojas.profilio_nuotrauka : null));

            // DI Komentaras mappings
            CreateMap<Dirbtinio_intelekto_komentaras, DIKomentarasDto>();

            // Citata mappings
            CreateMap<Citata, CitataDto>();

            // Irasas mappings
            CreateMap<Irasas, IrasasDto>()
                .ForMember(dest => dest.Knyga, opt => opt.MapFrom(src => src.Knyga));

            // Tema mappings
            CreateMap<Tema, TemaListDto>()
                .ForMember(dest => dest.autorius_slapyvardis, opt => opt.MapFrom(src =>
                    src.Naudotojas != null ? src.Naudotojas.slapyvardis : ""));

            CreateMap<Tema, TemaDetailDto>()
                .ForMember(dest => dest.autorius_slapyvardis, opt => opt.MapFrom(src =>
                    src.Naudotojas != null ? src.Naudotojas.slapyvardis : ""))
                .ForMember(dest => dest.autorius_nuotrauka, opt => opt.MapFrom(src =>
                    src.Naudotojas != null ? src.Naudotojas.profilio_nuotrauka : null));

            // Sekimas mappings
            CreateMap<Autoriaus_sekimas, SekimasDto>()
                .ForMember(dest => dest.Autorius, opt => opt.MapFrom(src => src.Autorius));
        }
    }
}
