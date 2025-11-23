namespace lentynaBackEnd.DTOs.Knygos
{
    public class IsplestinePaieskaDto
    {
        public string? ScenarijausAprasymas { get; set; }
        public List<Guid>? ZanruIds { get; set; }
        public List<Guid>? NuotaikuIds { get; set; }
    }

    public class KnygaSearchDto
    {
        public Guid Id { get; set; }
        public string Pavadinimas { get; set; } = string.Empty;
        public string? Aprasymas { get; set; }
        public string Zanras { get; set; } = string.Empty;
    }
}
