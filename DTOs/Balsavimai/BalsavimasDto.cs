using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.DTOs.Balsavimai
{
    public class BalsavimasDto
    {
        public Guid Id { get; set; }
        public DateTime balsavimo_pradzia { get; set; }
        public DateTime balsavimo_pabaiga { get; set; }
        public bool uzbaigtas { get; set; }
        public KnygaListDto? isrinkta_knyga { get; set; }
        public List<KnygaBalsuDto> nominuotos_knygos { get; set; } = new();
        public int viso_balsu { get; set; }
    }

    public class KnygaBalsuDto
    {
        public Guid Id { get; set; }
        public string knygos_pavadinimas { get; set; } = string.Empty;
        public string? virselio_nuotrauka { get; set; }
        public string autorius_vardas { get; set; } = string.Empty;
        public int balsu_skaicius { get; set; }
    }
}
