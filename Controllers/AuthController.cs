using System.Security.Claims;
using lentynaBackEnd.DTOs.Auth;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IFileUploadService _fileUploadService;

        public AuthController(IAuthService authService, IFileUploadService fileUploadService)
        {
            _authService = authService;
            _fileUploadService = fileUploadService;
        }

        [HttpPost("registruotis")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var (result, response) = await _authService.RegisterAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(response);
        }

        [HttpPost("prisijungti")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (result, response) = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(response);
        }

        [HttpGet("profilis")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, profile) = await _authService.GetProfileAsync(userId.Value);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(profile);
        }

        [HttpPut("profilis")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, profile) = await _authService.UpdateProfileAsync(userId.Value, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(profile);
        }

        [HttpDelete("profilis")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _authService.DeleteAccountAsync(userId.Value);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpPut("naudotojai/{id}/role")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto dto)
        {
            var result = await _authService.UpdateRoleAsync(id, dto.role);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpGet("naudotojai")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var naudotojai = await _authService.GetAllUsersAsync();
            return Ok(naudotojai);
        }

        /// <summary>
        /// Įkelti profilio nuotrauką
        /// </summary>
        [HttpPost("profilis/nuotrauka")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
        {
            var (success, url, error) = await _fileUploadService.UploadImageAsync(file, "images/profiliai");

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { url });
        }

        /// <summary>
        /// Ištrinti profilio nuotrauką
        /// </summary>
        [HttpDelete("profilis/nuotrauka")]
        [Authorize]
        public async Task<IActionResult> DeleteProfilePhoto()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var (result, profile) = await _authService.GetProfileAsync(userId.Value);
            if (!result.IsSuccess || string.IsNullOrEmpty(profile?.profilio_nuotrauka))
            {
                return NotFound(new { message = "Profilio nuotrauka nerasta" });
            }

            var deleted = _fileUploadService.DeleteImage(profile.profilio_nuotrauka);
            if (!deleted)
            {
                return NotFound(new { message = "Nuotrauka nerasta" });
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
