using System.ComponentModel.DataAnnotations;

namespace lentynaBackEnd.DTOs.NuomoniuForumas
{
    public class UpdateTemaDto
    {
        [MaxLength(255)]
        public string? pavadinimas { get; set; }

        public string? tekstas { get; set; }
    }
}
