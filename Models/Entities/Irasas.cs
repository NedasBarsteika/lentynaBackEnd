using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.Models.Entities
{
    public class Irasas
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public BookshelfTypes tipas { get; set; }

        [Required]
        public DateTime sukurimo_data { get; set; } = DateTime.UtcNow;

        public DateTime? redagavimo_data { get; set; }

        // Foreign keys
        public Guid NaudotojasId { get; set; }

        [ForeignKey("NaudotojasId")]
        public Naudotojas? Naudotojas { get; set; }

        public Guid KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga? Knyga { get; set; }

        public Guid? KnygosRekomendacijaId { get; set; }

        [ForeignKey("KnygosRekomendacijaId")]
        public Knygos_rekomendacija? Knygos_rekomendacija { get; set; }
    }
}
