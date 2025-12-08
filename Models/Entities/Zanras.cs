using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.Models.Entities
{
    public class Zanras
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string pavadinimas { get; set; } = string.Empty;

        // Navigation properties
        public List<Knyga> Knygos { get; set; } = new();
        // PASTABA: Nuotaikos nebėra kaip navigation property,
        // nes Nuotaika saugo ZanrasIds kaip JSON stulpelį
        // Užklausai: context.Nuotaikos.Where(n => n.ZanrasIds.Contains(zanrasId))
    }
}
