using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/nuotaikos")]
    public class NuotaikosController : ControllerBase
    {
        private readonly INuotaikaService _nuotaikaService;

        public NuotaikosController(INuotaikaService nuotaikaService)
        {
            _nuotaikaService = nuotaikaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var nuotaikos = await _nuotaikaService.GetAllAsync();
            return Ok(nuotaikos);
        }

        [HttpPost]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Create([FromBody] CreateNuotaikaDto dto)
        {
            var (result, nuotaika) = await _nuotaikaService.CreateAsync(dto.pavadinimas, dto.ZanrasIds);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(nuotaika);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _nuotaikaService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }
    }

    public class CreateNuotaikaDto
    {
        public string pavadinimas { get; set; } = string.Empty;
        public List<Guid> ZanrasIds { get; set; } = new();
    }
}
