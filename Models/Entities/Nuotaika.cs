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

        // Saugome kaip JSON stulpelį duomenų bazėje
        public List<Guid> ZanrasIds { get; set; } = new();
    }
}
