using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Citata
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string citatos_tekstas { get; set; } = string.Empty;

        public DateTime? citatos_data { get; set; }

        [MaxLength(255)]
        public string? citatos_saltinis { get; set; }

        // Foreign key
        public Guid AutoriusId { get; set; }

        [ForeignKey("AutoriusId")]
        public Autorius? Autorius { get; set; }
    }
}
