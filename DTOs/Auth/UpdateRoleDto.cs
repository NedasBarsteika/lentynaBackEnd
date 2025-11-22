using System.ComponentModel.DataAnnotations;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.DTOs.Auth
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "Rolė yra privaloma")]
        public Roles role { get; set; }
    }
}
