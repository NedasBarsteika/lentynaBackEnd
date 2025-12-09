using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Balsavimas
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime balsavimo_pradzia { get; set; }

        [Required]
        public DateTime balsavimo_pabaiga { get; set; }

        public Guid? isrinkta_knyga_id { get; set; }

        [ForeignKey("isrinkta_knyga_id")]
        public Knyga? IsrinktaKnyga { get; set; }

        public bool uzbaigtas { get; set; } = false;

        // Navigation properties
        public List<Balsas> Balsai { get; set; } = new();
        public List<BalsavimoKnyga> BalsavimoKnygos { get; set; } = new();
    }
}
