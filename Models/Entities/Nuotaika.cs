using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.Models.Entities
{
    public class Nuotaika
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string pavadinimas { get; set; } = string.Empty;

        // Navigation properties
        public List<NuotaikosZanras> NuotaikosZanrai { get; set; } = new();
    }
}
