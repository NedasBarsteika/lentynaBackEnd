using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class KnygaZanras
    {
        public Guid KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga? Knyga { get; set; }

        public Guid ZanrasId { get; set; }

        [ForeignKey("ZanrasId")]
        public Zanras? Zanras { get; set; }
    }
}
