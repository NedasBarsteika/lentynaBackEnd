using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Knygos
{
    public class UpdateKnygaDto
    {
        [MaxLength(255)]
        public string? knygos_pavadinimas { get; set; }

        public DateTime? leidimo_metai { get; set; }

        public string? aprasymas { get; set; }

        public int? psl_skaicius { get; set; }

        [MaxLength(20)]
        public string? ISBN { get; set; }

        [MaxLength(500)]
        public string? virselio_nuotrauka { get; set; }

        [MaxLength(50)]
        public string? kalba { get; set; }

        public bool? bestseleris { get; set; }

        public Guid? AutoriusId { get; set; }

        public Guid? ZanrasId { get; set; }
    }
}
