using System.Security.Claims;
using lentynaBackEnd.DTOs.Irasai;
using lentynaBackEnd.Models.Enums;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/irasai")]
    [Authorize]
    public class IrasaiController : ControllerBase
    {
        private readonly IIrasasService _irasasService;

        public IrasaiController(IIrasasService irasasService)
        {
            _irasasService = irasasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookshelfTypes? tipas = null)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (tipas.HasValue)
            {
                var irasai = await _irasasService.GetByNaudotojasIdAndTipasAsync(userId.Value, tipas.Value);
                return Ok(irasai);
            }
            else
            {
                var irasai = await _irasasService.GetByNaudotojasIdAsync(userId.Value);
                return Ok(irasai);
            }
        }

        [HttpGet("rekomendacijos")]
        public async Task<IActionResult> GetRekomendacijos()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var rekomendacijos = await _irasasService.GetRekomendacijosAsync(userId.Value);
            return Ok(rekomendacijos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIrasasDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, irasas) = await _irasasService.CreateAsync(userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(irasas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIrasasDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, irasas) = await _irasasService.UpdateAsync(id, userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(irasas);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _irasasService.DeleteAsync(id, userId.Value);

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
