using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.DTOs.Autoriai
{
    public class AutoriusDetailDto
    {
        public Guid Id { get; set; }
        public string vardas { get; set; } = string.Empty;
        public string pavarde { get; set; } = string.Empty;
        public DateTime? gimimo_metai { get; set; }
        public DateTime? mirties_data { get; set; }
        public string? curiculum_vitae { get; set; }
        public string? nuotrauka { get; set; }
        public string? laidybe { get; set; }
        public int knygu_skaicius { get; set; }
        public List<KnygaListDto> knygos { get; set; } = new();
        public List<CitataDto> citatos { get; set; } = new();
        public int sekejuSkaicius { get; set; }
    }

    public class CitataDto
    {
        public Guid Id { get; set; }
        public string citatos_tekstas { get; set; } = string.Empty;
        public DateTime? citatos_data { get; set; }
        public string? citatos_saltinis { get; set; }
    }
}
