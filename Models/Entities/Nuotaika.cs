using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class Nuotaika
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string pavadinimas { get; set; } = string.Empty;

        // Foreign key
        public Guid ZanrasId { get; set; }

        [ForeignKey("ZanrasId")]
        public Zanras? Zanras { get; set; }
    }
}
