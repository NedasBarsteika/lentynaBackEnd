using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.Models.Entities
{
    public class Autorius
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string vardas { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string pavarde { get; set; } = string.Empty;

        public DateTime? gimimo_metai { get; set; }

        public DateTime? mirties_data { get; set; }

        public string? curiculum_vitae { get; set; }

        [MaxLength(500)]
        public string? nuotrauka { get; set; }

        [MaxLength(100)]
        public string? tautybe { get; set; }

        public int knygu_skaicius { get; set; } = 0;

        // Navigation properties
        public List<Knyga> Knygos { get; set; } = new();
        public List<Citata> Citatos { get; set; } = new();
        public List<Autoriaus_sekimas> Autoriaus_sekimai { get; set; } = new();
    }
}
