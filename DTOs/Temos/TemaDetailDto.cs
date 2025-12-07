using lentynaBackEnd.DTOs.Komentarai;

namespace lentynaBackEnd.DTOs.Temos
{
    public class TemaDetailDto
    {
        public Guid Id { get; set; }
        public string pavadinimas { get; set; } = string.Empty;
        public string tekstas { get; set; } = string.Empty;
        public DateTime sukurimo_data { get; set; }
        public DateTime? redagavimo_data { get; set; }
        public Guid NaudotojasId { get; set; }
        public string autorius_slapyvardis { get; set; } = string.Empty;
        public string? autorius_nuotrauka { get; set; }
        public List<KomentarasDto> komentarai { get; set; } = new();
    }
}
