using System.Security.Claims;
using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.NuomoniuForumas;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/nuomoniu-forumas")]
    public class NuomoniuForumasController : ControllerBase
    {
        private readonly INuomoniuForumasService _nuomoniuForumasService;

        public NuomoniuForumasController(INuomoniuForumasService nuomoniuForumasService)
        {
            _nuomoniuForumasService = nuomoniuForumasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = Constants.DefaultPage,
            [FromQuery] int pageSize = Constants.DefaultPageSize)
        {
            var result = await _nuomoniuForumasService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (result, tema) = await _nuomoniuForumasService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(tema);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTemaDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, tema) = await _nuomoniuForumasService.CreateAsync(userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = tema!.Id }, tema);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTemaDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, tema) = await _nuomoniuForumasService.UpdateAsync(id, userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(tema);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var isModerator = User.IsInRole("moderatorius") || User.IsInRole("admin");
            var result = await _nuomoniuForumasService.DeleteAsync(id, userId.Value, isModerator);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return null;
            }
            return userId;
        }
    }
}
