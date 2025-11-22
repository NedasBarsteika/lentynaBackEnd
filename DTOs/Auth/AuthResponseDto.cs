namespace lentynaBackEnd.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string token { get; set; } = string.Empty;
        public NaudotojasDto naudotojas { get; set; } = null!;
    }

    public class NaudotojasDto
    {
        public Guid Id { get; set; }
        public string slapyvardis { get; set; } = string.Empty;
        public string el_pastas { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public DateTime sukurimo_data { get; set; }
        public string? profilio_nuotrauka { get; set; }
    }
}
