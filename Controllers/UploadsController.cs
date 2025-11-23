using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/uploads")]
    public class UploadsController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public UploadsController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Įkelti nuotrauką (bendrai naudojimui)
        /// </summary>
        [HttpPost("image")]
        [Authorize]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var (success, url, error) = await _fileUploadService.UploadImageAsync(file, "images");

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { url });
        }

        /// <summary>
        /// Įkelti knygos viršelio nuotrauką
        /// </summary>
        [HttpPost("knygos/virselis")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> UploadBookCover(IFormFile file)
        {
            var (success, url, error) = await _fileUploadService.UploadImageAsync(file, "images/knygos");

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { url });
        }

        /// <summary>
        /// Įkelti autoriaus nuotrauką
        /// </summary>
        [HttpPost("autoriai/nuotrauka")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> UploadAuthorPhoto(IFormFile file)
        {
            var (success, url, error) = await _fileUploadService.UploadImageAsync(file, "images/autoriai");

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { url });
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
        /// Ištrinti nuotrauką
        /// </summary>
        [HttpDelete]
        [Authorize(Roles = "redaktorius,admin")]
        public IActionResult DeleteImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest(new { message = "URL nepateiktas" });
            }

            var deleted = _fileUploadService.DeleteImage(url);

            if (!deleted)
            {
                return NotFound(new { message = "Nuotrauka nerasta" });
            }

            return NoContent();
        }
    }
}
