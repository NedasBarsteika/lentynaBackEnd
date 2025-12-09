using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.KnyguKlubas
{
    public class CreateBalsavimasDto
    {
        [Required(ErrorMessage = "Balsavimo pradžia yra privaloma")]
        public DateTime balsavimo_pradzia { get; set; }

        [Required(ErrorMessage = "Balsavimo pabaiga yra privaloma")]
        public DateTime balsavimo_pabaiga { get; set; }

        public DateTime? susitikimo_data { get; set; }

        [Required(ErrorMessage = "Nominuotos knygos yra privalomos")]
        [MinLength(1, ErrorMessage = "Turi būti bent viena nominuota knyga")]
        public List<Guid> nominuotos_knygos { get; set; } = new();
    }
}
