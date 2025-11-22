using System.Security.Claims;
using lentynaBackEnd.DTOs.Sekimai;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/sekimai")]
    [Authorize]
    public class SekimaiController : ControllerBase
    {
        private readonly ISekimasService _sekimasService;

        public SekimaiController(ISekimasService sekimasService)
        {
            _sekimasService = sekimasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var sekimai = await _sekimasService.GetByNaudotojasIdAsync(userId.Value);
            return Ok(sekimai);
        }

        [HttpGet("tikrinti/{autoriusId}")]
        public async Task<IActionResult> CheckFollowing(Guid autoriusId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var isFollowing = await _sekimasService.IsFollowingAsync(userId.Value, autoriusId);
            return Ok(new { isFollowing });
        }

        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] CreateSekimasDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, sekimas) = await _sekimasService.FollowAsync(userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(sekimas);
        }

        [HttpDelete("{autoriusId}")]
        public async Task<IActionResult> Unfollow(Guid autoriusId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _sekimasService.UnfollowAsync(userId.Value, autoriusId);

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
