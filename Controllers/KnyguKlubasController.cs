using System.Security.Claims;
using lentynaBackEnd.DTOs.KnyguKlubas;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/knygu-klubas")]
    public class KnyguKlubasController : ControllerBase
    {
        private readonly IKnyguKlubasService _knyguKlubasService;

        public KnyguKlubasController(IKnyguKlubasService knyguKlubasService)
        {
            _knyguKlubasService = knyguKlubasService;
        }

        [HttpGet("dabartinis")]
        public async Task<IActionResult> GetCurrent()
        {
            var (result, balsavimasDto) = await _knyguKlubasService.GetCurrentAsync();

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }


            return Ok(balsavimasDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (result, balsavimas) = await _knyguKlubasService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(balsavimas);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateBalsavimasDto dto)
        {
            var (result, balsavimas) = await _knyguKlubasService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = balsavimas!.Id }, balsavimas);
        }

        [HttpGet("{id}/oro-prognoze")]
        public async Task<IActionResult> GetOroPrognoze(Guid id)
        {
            var (result, oroPrognoze) = await _knyguKlubasService.GetOroPrognozeAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { oro_prognoze = oroPrognoze });
        }

        [HttpGet("{id}/mano-balsas")]
        [Authorize]
        public async Task<IActionResult> GetMyVote(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, votedBookId) = await _knyguKlubasService.GetUserVoteAsync(id, userId.Value);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { balsuota = votedBookId != null, knygaId = votedBookId });
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

    [ApiController]
    [Route("api/knygu-klubas/balsai")]
    public class KnyguKlubasBalsaiController : ControllerBase
    {
        private readonly IKnyguKlubasService _knyguKlubasService;

        public KnyguKlubasBalsaiController(IKnyguKlubasService knyguKlubasService)
        {
            _knyguKlubasService = knyguKlubasService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Vote([FromBody] CreateBalsasDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, success) = await _knyguKlubasService.VoteAsync(userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveVote(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _knyguKlubasService.RemoveVoteAsync(id, userId.Value);

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
