using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Tema
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string pavadinimas { get; set; } = string.Empty;

        [Required]
        public string tekstas { get; set; } = string.Empty;

        [Required]
        public DateTime sukurimo_data { get; set; } = DateTime.UtcNow;

        public DateTime? redagavimo_data { get; set; }

        public DateTime? istrynimo_data { get; set; }

        public bool prikabinta { get; set; } = false;

        // Foreign key
        public Guid NaudotojasId { get; set; }

        [ForeignKey("NaudotojasId")]
        public Naudotojas? Naudotojas { get; set; }

        // Navigation properties
        public List<Komentaras> Komentarai { get; set; } = new();
    }
}
