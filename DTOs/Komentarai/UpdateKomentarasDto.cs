using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Komentarai
{
    public class UpdateKomentarasDto
    {
        public string? komentaro_tekstas { get; set; }

        [Range(1, 5, ErrorMessage = "Vertinimas turi būti nuo 1 iki 5")]
        public int? vertinimas { get; set; }
    }
}
