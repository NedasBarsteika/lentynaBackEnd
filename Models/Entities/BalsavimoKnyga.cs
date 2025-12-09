using System.ComponentModel.DataAnnotations.Schema;

namespace lentynaBackEnd.Models.Entities
{
    public class BalsavimoKnyga
    {
        public Guid BalsavimasId { get; set; }

        [ForeignKey("BalsavimasId")]
        public Balsavimas Balsavimas { get; set; } = null!;

        public Guid KnygaId { get; set; }

        [ForeignKey("KnygaId")]
        public Knyga Knyga { get; set; } = null!;
    }
}
