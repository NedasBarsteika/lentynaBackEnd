using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Knygos
{
    public class CreateKnygaDto
    {
        [Required(ErrorMessage = "Knygos pavadinimas yra privalomas")]
        [MaxLength(255)]
        public string knygos_pavadinimas { get; set; } = string.Empty;

        public DateTime? leidimo_metai { get; set; }

        public string? aprasymas { get; set; }

        public int? psl_skaicius { get; set; }

        [MaxLength(20)]
        public string? ISBN { get; set; }

        [MaxLength(500)]
        public string? virselio_nuotrauka { get; set; }

        [MaxLength(50)]
        public string? kalba { get; set; }

        public bool bestseleris { get; set; } = false;

        [Required(ErrorMessage = "Autorius yra privalomas")]
        public Guid AutoriusId { get; set; }

        [Required(ErrorMessage = "Zanras yra privalomas")]
        public Guid ZanrasId { get; set; }
    }
}
