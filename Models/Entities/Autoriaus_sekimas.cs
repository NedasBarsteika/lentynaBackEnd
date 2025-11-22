using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Autoriaus_sekimas
    {
        public Guid NaudotojasId { get; set; }

        [ForeignKey("NaudotojasId")]
        public Naudotojas? Naudotojas { get; set; }

        public Guid AutoriusId { get; set; }

        [ForeignKey("AutoriusId")]
        public Autorius? Autorius { get; set; }

        [Required]
        public DateTime sekimo_pradzia { get; set; } = DateTime.UtcNow;
    }
}
