using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Auth
{
    public class UpdateProfileDto
    {
        [MaxLength(100)]
        public string? slapyvardis { get; set; }

        [MaxLength(500)]
        public string? profilio_nuotrauka { get; set; }
    }
}
