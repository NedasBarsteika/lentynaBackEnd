using System.ComponentModel.DataAnnotations;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.DTOs.Irasai
{
    public class UpdateIrasasDto
    {
        [Required(ErrorMessage = "Tipas yra privalomas")]
        public BookshelfTypes tipas { get; set; }
    }
}
