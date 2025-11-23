namespace lentynaBackEnd.DTOs.Knygos
{
    public class KnygaFilterDto
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? paieska { get; set; }
        public Guid? zanrasId { get; set; }
        public Guid? autoriusId { get; set; }
        public bool? bestseleris { get; set; }
        public string? sortBy { get; set; } = "pavadinimas";
        public bool descending { get; set; } = false;
    }
}
