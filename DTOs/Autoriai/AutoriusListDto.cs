namespace lentynaBackEnd.DTOs.Autoriai
{
    public class AutoriusListDto
    {
        public Guid Id { get; set; }
        public string vardas { get; set; } = string.Empty;
        public string pavarde { get; set; } = string.Empty;
        public string? curiculum_vitae { get; set; }
        public string? nuotrauka { get; set; }
        public int knygu_skaicius { get; set; }
    }
}
