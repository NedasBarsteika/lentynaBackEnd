using System.ComponentModel.DataAnnotations;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.DTOs.Irasai
{
    public class CreateIrasasDto
    {
        [Required(ErrorMessage = "Knygos ID yra privalomas")]
        public Guid KnygaId { get; set; }

        [Required(ErrorMessage = "Tipas yra privalomas")]
        public BookshelfTypes tipas { get; set; }
    }
}
