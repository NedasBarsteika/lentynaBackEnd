using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Dirbtinio_intelekto_komentaras
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime sugeneravimo_data { get; set; } = DateTime.UtcNow;

        [Required]
        public string tekstas { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string modelis { get; set; } = string.Empty;

        // Foreign key
        public Guid KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga? Knyga { get; set; }
    }
}
