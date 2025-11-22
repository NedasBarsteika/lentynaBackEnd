using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Sekimai
{
    public class CreateSekimasDto
    {
        [Required(ErrorMessage = "Autoriaus ID yra privalomas")]
        public Guid AutoriusId { get; set; }
    }
}
