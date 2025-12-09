namespace lentynaBackEnd.DTOs.Komentarai
{
    public class KomentarasDto
    {
        public Guid Id { get; set; }
        public string komentaro_tekstas { get; set; } = string.Empty;
        public DateTime komentaro_data { get; set; }
        public int vertinimas { get; set; }
        public DateTime? redagavimo_data { get; set; }
        public Guid NaudotojasId { get; set; }
        public string naudotojo_slapyvardis { get; set; } = string.Empty;
        public string? naudotojo_nuotrauka { get; set; }
        public Guid? KnygaId { get; set; }
    }

     public class DIKomentarasDto
    {
        public Guid Id { get; set; }
        public DateTime sugeneravimo_data { get; set; }
        public string tekstas { get; set; } = string.Empty;
        public string modelis { get; set; } = string.Empty;
    }
}
