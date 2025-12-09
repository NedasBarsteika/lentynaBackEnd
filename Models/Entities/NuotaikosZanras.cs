using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class NuotaikosZanras
    {
        public Guid NuotaikaId { get; set; }

        [ForeignKey("NuotaikaId")]
        public Nuotaika Nuotaika { get; set; } = null!;

        public Guid ZanrasId { get; set; }

        [ForeignKey("ZanrasId")]
        public Zanras Zanras { get; set; } = null!;
    }
}
