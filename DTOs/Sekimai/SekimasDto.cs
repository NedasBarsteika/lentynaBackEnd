using lentynaBackEnd.DTOs.Autoriai;

namespace lentynaBackEnd.DTOs.Sekimai
{
    public class SekimasDto
    {
        public Guid AutoriusId { get; set; }
        public DateTime sekimo_pradzia { get; set; }
        public AutoriusListDto Autorius { get; set; } = null!;
    }
}
