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
        public List<KnygaZanras> KnygaZanrai { get; set; } = new();
    }
}
