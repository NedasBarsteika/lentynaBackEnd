using System.Security.Claims;
using lentynaBackEnd.DTOs.Komentarai;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/komentarai")]
    public class KomentaraiController : ControllerBase
    {
        private readonly IKomentarasService _komentarasService;

        public KomentaraiController(IKomentarasService komentarasService)
        {
            _komentarasService = komentarasService;
        }

        [HttpGet("knyga/{knygaId}")]
        public async Task<IActionResult> GetByKnygaId(Guid knygaId)
        {
            var komentarai = await _komentarasService.GetByKnygaIdAsync(knygaId);
            return Ok(komentarai);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateKomentarasDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, komentaras) = await _komentarasService.CreateAsync(userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(komentaras);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateKomentarasDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, komentaras) = await _komentarasService.UpdateAsync(id, userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(komentaras);
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

            var isAdmin = User.IsInRole("admin") || User.IsInRole("moderatorius");
            var result = await _komentarasService.DeleteAsync(id, userId.Value, isAdmin);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpGet("komentarai/{id}")]
        public async Task<IActionResult> GetKomentarai(Guid id)
        {
            var komentarai = await _komentarasService.GetByKnygaIdAsync(id);
            return Ok(komentarai);
        }

        [HttpGet("dikomentaras/{id}")]
        public async Task<IActionResult> Getdikomentaras(Guid id)
        {
            var (result, komentaras) = await _komentarasService.GetDIComment(id);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(komentaras);
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
