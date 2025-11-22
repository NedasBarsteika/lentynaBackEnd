using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/zanrai")]
    public class ZanraiController : ControllerBase
    {
        private readonly IZanrasService _zanrasService;

        public ZanraiController(IZanrasService zanrasService)
        {
            _zanrasService = zanrasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var zanrai = await _zanrasService.GetAllAsync();
            return Ok(zanrai);
        }

        [HttpPost]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Create([FromBody] CreateZanrasDto dto)
        {
            var (result, zanras) = await _zanrasService.CreateAsync(dto.pavadinimas);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(zanras);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _zanrasService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }
    }

    public class CreateZanrasDto
    {
        public string pavadinimas { get; set; } = string.Empty;
    }
}
