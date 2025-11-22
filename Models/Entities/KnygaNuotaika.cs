using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class KnygaNuotaika
    {
        public Guid KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga? Knyga { get; set; }

        public Guid NuotaikaId { get; set; }

        [ForeignKey("NuotaikaId")]
        public Nuotaika? Nuotaika { get; set; }
    }
}
