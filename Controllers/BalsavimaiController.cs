using System.Security.Claims;
using lentynaBackEnd.DTOs.Balsavimai;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/balsavimai")]
    public class BalsavimaiController : ControllerBase
    {
        private readonly IBalsavimasService _balsavimasService;

        public BalsavimaiController(IBalsavimasService balsavimasService)
        {
            _balsavimasService = balsavimasService;
        }

        [HttpGet("dabartinis")]
        public async Task<IActionResult> GetCurrent()
        {
            var (result, balsavimas) = await _balsavimasService.GetCurrentAsync();

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(balsavimas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (result, balsavimas) = await _balsavimasService.GetByIdAsync(id);

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
            var (result, balsavimas) = await _balsavimasService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = balsavimas!.Id }, balsavimas);
        }

        [HttpGet("{id}/oro-prognoze")]
        public async Task<IActionResult> GetOroPrognoze(Guid id)
        {
            var (result, oroPrognoze) = await _balsavimasService.GetOroPrognozeAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { oro_prognoze = oroPrognoze });
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
    [Route("api/balsai")]
    public class BalsaiController : ControllerBase
    {
        private readonly IBalsavimasService _balsavimasService;

        public BalsaiController(IBalsavimasService balsavimasService)
        {
            _balsavimasService = balsavimasService;
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

            var (result, success) = await _balsavimasService.VoteAsync(userId.Value, dto);

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

            var result = await _balsavimasService.RemoveVoteAsync(id, userId.Value);

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
