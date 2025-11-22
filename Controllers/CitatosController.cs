using lentynaBackEnd.DTOs.Citatos;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/citatos")]
    public class CitatosController : ControllerBase
    {
        private readonly ICitataService _citataService;

        public CitatosController(ICitataService citataService)
        {
            _citataService = citataService;
        }

        [HttpGet("autorius/{autoriusId}")]
        public async Task<IActionResult> GetByAutoriusId(Guid autoriusId)
        {
            var citatos = await _citataService.GetByAutoriusIdAsync(autoriusId);
            return Ok(citatos);
        }

        [HttpPost]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Create([FromBody] CreateCitataDto dto)
        {
            var (result, citata) = await _citataService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(citata);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _citataService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }
    }
}
