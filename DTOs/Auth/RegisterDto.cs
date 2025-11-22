using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Slapyvardis yra privalomas")]
        [MaxLength(100)]
        public string slapyvardis { get; set; } = string.Empty;

        [Required(ErrorMessage = "El. paštas yra privalomas")]
        [EmailAddress(ErrorMessage = "Neteisingas el. pašto formatas")]
        [MaxLength(255)]
        public string el_pastas { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slaptažodis yra privalomas")]
        [MinLength(6, ErrorMessage = "Slaptažodis turi būti bent 6 simbolių")]
        public string slaptazodis { get; set; } = string.Empty;
    }
}
