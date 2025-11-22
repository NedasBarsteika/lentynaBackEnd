using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Komentaras
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string komentaro_tekstas { get; set; } = string.Empty;

        [Required]
        public DateTime komentaro_data { get; set; } = DateTime.UtcNow;

        [Required]
        [Range(1, 5)]
        public int vertinimas { get; set; }

        public DateTime? redagavimo_data { get; set; }

        // Foreign keys
        public Guid NaudotojasId { get; set; }

        [ForeignKey("NaudotojasId")]
        public Naudotojas? Naudotojas { get; set; }

        public Guid? KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga? Knyga { get; set; }

        public Guid? TemaId { get; set; }

        [ForeignKey("TemaId")]
        public Tema? Tema { get; set; }
    }
}
