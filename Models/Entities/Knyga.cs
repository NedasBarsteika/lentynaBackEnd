using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Knyga
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string knygos_pavadinimas { get; set; } = string.Empty;

        public DateTime? leidimo_metai { get; set; }

        public string? aprasymas { get; set; }

        public int? psl_skaicius { get; set; }

        [MaxLength(20)]
        public string? ISBN { get; set; }

        [MaxLength(500)]
        public string? virselio_nuotrauka { get; set; }

        [MaxLength(100)]
        public string? raisos { get; set; }

        public bool bestseleris { get; set; } = false;

        // Foreign key
        public Guid AutoriusId { get; set; }

        [ForeignKey("AutoriusId")]
        public Autorius? Autorius { get; set; }

        // Navigation properties
        public List<KnygaZanras> KnygaZanrai { get; set; } = new();
        public List<KnygaNuotaika> KnygaNuotaikos { get; set; } = new();
        public List<Komentaras> Komentarai { get; set; } = new();
        public List<Dirbtinio_intelekto_komentaras> DI_Komentarai { get; set; } = new();
        public List<Irasas> Irasai { get; set; } = new();
        public List<Balsas> Balsai { get; set; } = new();
    }
}
