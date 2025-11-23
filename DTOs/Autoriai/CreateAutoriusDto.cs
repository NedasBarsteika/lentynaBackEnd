using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.Autoriai
{
    public class CreateAutoriusDto
    {
        [Required(ErrorMessage = "Vardas yra privalomas")]
        [MaxLength(100)]
        public string vardas { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pavardė yra privaloma")]
        [MaxLength(100)]
        public string pavarde { get; set; } = string.Empty;

        public DateTime? gimimo_metai { get; set; }

        public DateTime? mirties_data { get; set; }

        public string? curiculum_vitae { get; set; }

        [MaxLength(500)]
        public string? nuotrauka { get; set; }

        [MaxLength(100)]
        public string? tautybe { get; set; }
    }
}
