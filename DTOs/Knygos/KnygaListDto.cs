namespace lentynaBackEnd.DTOs.Knygos
{
    public class KnygaListDto
    {
        public Guid Id { get; set; }
        public string knygos_pavadinimas { get; set; } = string.Empty;
        public DateTime? leidimo_metai { get; set; }
        public string? virselio_nuotrauka { get; set; }
        public bool bestseleris { get; set; }
        public string autorius_vardas { get; set; } = string.Empty;
        public string zanras { get; set; } = string.Empty;
        public double vidutinis_vertinimas { get; set; }
        public int komentaru_skaicius { get; set; }
    }
}
