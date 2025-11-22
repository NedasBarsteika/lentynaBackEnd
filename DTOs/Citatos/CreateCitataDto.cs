using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Citatos
{
    public class CreateCitataDto
    {
        [Required(ErrorMessage = "Citatos tekstas yra privalomas")]
        public string citatos_tekstas { get; set; } = string.Empty;

        public DateTime? citatos_data { get; set; }

        [MaxLength(255)]
        public string? citatos_saltinis { get; set; }

        [Required(ErrorMessage = "Autoriaus ID yra privalomas")]
        public Guid AutoriusId { get; set; }
    }
}
