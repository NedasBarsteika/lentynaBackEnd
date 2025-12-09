namespace lentynaBackEnd.DTOs.Knygos
{
    public class KnygaDetailDto
    {
        public Guid Id { get; set; }
        public string knygos_pavadinimas { get; set; } = string.Empty;
        public DateTime? leidimo_metai { get; set; }
        public string? aprasymas { get; set; }
        public int? psl_skaicius { get; set; }
        public string? ISBN { get; set; }
        public string? virselio_nuotrauka { get; set; }
        public string? kalba { get; set; }
        public bool bestseleris { get; set; }
        public Guid AutoriusId { get; set; }
        public AutoriusSummaryDto Autorius { get; set; } = null!;
        public Guid ZanrasId { get; set; }
        public ZanrasDto Zanras { get; set; } = null!;
        public double vidutinis_vertinimas { get; set; }
        public int komentaru_skaicius { get; set; }
    }

    public class AutoriusSummaryDto
    {
        public Guid Id { get; set; }
        public string vardas { get; set; } = string.Empty;
        public string pavarde { get; set; } = string.Empty;
        public string? nuotrauka { get; set; }
    }

    public class ZanrasDto
    {
        public Guid Id { get; set; }
        public string pavadinimas { get; set; } = string.Empty;
    }

    public class NuotaikaDto
    {
        public Guid Id { get; set; }
        public string pavadinimas { get; set; } = string.Empty;
        public List<ZanrasDto> Zanrai { get; set; } = new();
    }

}
