using System.ComponentModel.DataAnnotations;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.Models.Entities
{
    public class Naudotojas
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string slapyvardis { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string el_pastas { get; set; } = string.Empty;

        [Required]
        public string slaptazodis { get; set; } = string.Empty;

        [Required]
        public Roles role { get; set; } = Roles.naudotojas;

        public DateTime sukurimo_data { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? profilio_nuotrauka { get; set; }

        // Navigation properties
        public List<Komentaras> Komentarai { get; set; } = new();
        public List<Irasas> Irasai { get; set; } = new();
        public List<Tema> Temos { get; set; } = new();
        public List<Balsas> Balsai { get; set; } = new();
        public List<Knygos_rekomendacija> Knygos_rekomendacijos { get; set; } = new();
        public List<Autoriaus_sekimas> Autoriaus_sekimai { get; set; } = new();
    }
}
