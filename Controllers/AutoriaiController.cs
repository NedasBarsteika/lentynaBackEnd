using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/autoriai")]
    public class AutoriaiController : ControllerBase
    {
        private readonly IAutoriusService _autoriusService;

        public AutoriaiController(IAutoriusService autoriusService)
        {
            _autoriusService = autoriusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = Constants.DefaultPage,
            [FromQuery] int pageSize = Constants.DefaultPageSize)
        {
            var result = await _autoriusService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (result, autorius) = await _autoriusService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(autorius);
        }

        [HttpPost]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Create([FromBody] CreateAutoriusDto dto)
        {
            var (result, autorius) = await _autoriusService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = autorius!.Id }, autorius);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAutoriusDto dto)
        {
            var (result, autorius) = await _autoriusService.UpdateAsync(id, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(autorius);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _autoriusService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpGet("{id}/knygos")]
        public async Task<IActionResult> GetKnygos(Guid id)
        {
            var knygos = await _autoriusService.GetKnygosAsync(id);
            return Ok(knygos);
        }

        [HttpGet("{id}/citatos")]
        public async Task<IActionResult> GetCitatos(Guid id)
        {
            var citatos = await _autoriusService.GetCitatosAsync(id);
            return Ok(citatos);
        }
    }
}
