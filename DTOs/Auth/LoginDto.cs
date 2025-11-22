using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El. paštas yra privalomas")]
        [EmailAddress(ErrorMessage = "Neteisingas el. pašto formatas")]
        public string el_pastas { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slaptažodis yra privalomas")]
        public string slaptazodis { get; set; } = string.Empty;
    }
}
