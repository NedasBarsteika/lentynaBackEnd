using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Knygos_rekomendacija
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime rekomendacijos_pradzia { get; set; } = DateTime.UtcNow;

        public DateTime? rekomendacijos_pabaiga { get; set; }

        // Foreign key
        public Guid NaudotojasId { get; set; }

        [ForeignKey("NaudotojasId")]
        public Naudotojas? Naudotojas { get; set; }

        // Navigation properties
        public List<Irasas> Irasai { get; set; } = new();
    }
}
