using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Balsas
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime pateikimo_data { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public Guid NaudotojasId { get; set; }

        [ForeignKey("NaudotojasId")]
        public Naudotojas? Naudotojas { get; set; }

        public Guid BalsavimasId { get; set; }

        [ForeignKey("BalsavimasId")]
        public Balsavimas? Balsavimas { get; set; }

        public Guid KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga? Knyga { get; set; }
    }
}
