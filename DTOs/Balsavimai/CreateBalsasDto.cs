using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Balsavimai
{
    public class CreateBalsasDto
    {
        [Required(ErrorMessage = "Balsavimo ID yra privalomas")]
        public Guid BalsavimasId { get; set; }

        [Required(ErrorMessage = "Knygos ID yra privalomas")]
        public Guid KnygaId { get; set; }
    }
}
