using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Temos
{
    public class CreateTemaDto
    {
        [Required(ErrorMessage = "Pavadinimas yra privalomas")]
        [MaxLength(255)]
        public string pavadinimas { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tekstas yra privalomas")]
        public string tekstas { get; set; } = string.Empty;
    }
}
