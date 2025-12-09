using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Komentarai
{
    public class CreateKomentarasDto
    {
        [Required(ErrorMessage = "Komentaro tekstas yra privalomas")]
        public string komentaro_tekstas { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vertinimas yra privalomas")]
        [Range(1, 5, ErrorMessage = "Vertinimas turi būti nuo 1 iki 5")]
        public int vertinimas { get; set; }

        public Guid? KnygaId { get; set; }
    }
}
