using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.DTOs.Irasai
{
    public class IrasasDto
    {
        public Guid Id { get; set; }
        public BookshelfTypes tipas { get; set; }
        public DateTime sukurimo_data { get; set; }
        public DateTime? redagavimo_data { get; set; }
        public KnygaListDto Knyga { get; set; } = null!;
    }
}
