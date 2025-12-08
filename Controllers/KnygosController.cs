using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/knygos")]
    public class KnygosController : ControllerBase
    {
        private readonly IKnygaService _knygaService;
        private readonly IKomentarasService _komentarasService;
        private readonly IFileUploadService _fileUploadService;

        public KnygosController(IKnygaService knygaService, IKomentarasService komentarasService, IFileUploadService fileUploadService)
        {
            _knygaService = knygaService;
            _komentarasService = komentarasService;
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] KnygaFilterDto filter)
        {
            var result = await _knygaService.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (result, knyga) = await _knygaService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(knyga);
        }

        [HttpPost]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Create([FromBody] CreateKnygaDto dto)
        {
            var (result, knyga) = await _knygaService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = knyga!.Id }, knyga);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateKnygaDto dto)
        {
            var (result, knyga) = await _knygaService.UpdateAsync(id, dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(knyga);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _knygaService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpGet("{id}/komentarai")]
        public async Task<IActionResult> GetKomentarai(Guid id)
        {
            var komentarai = await _komentarasService.GetByKnygaIdAsync(id);
            return Ok(komentarai);
        }

        [HttpPost("isplestine-paieska")]
        public async Task<IActionResult> IsplestinePaieska([FromBody] IsplestinePaieskaDto dto)
        {
            try
            {
                var result = await _knygaService.IsplestinePaieskaAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Klaida atliekant AI paiešką", error = ex.Message });
            }
        }

        /// <summary>
        /// Įkelti knygos viršelio nuotrauką
        /// </summary>
        [HttpPost("{id}/virselis")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> UploadBookCover(Guid id, IFormFile file)
        {
            // Validuoti ar knyga egzistuoja
            var (result, knyga) = await _knygaService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(new { message = "Knyga nerasta" });
            }

            var (success, url, error) = await _fileUploadService.UploadImageAsync(file, "images/knygos");

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { url });
        }

        /// <summary>
        /// Ištrinti knygos viršelio nuotrauką
        /// </summary>
        [HttpDelete("{id}/virselis")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> DeleteBookCover(Guid id)
        {
            var (result, knyga) = await _knygaService.GetByIdAsync(id);
            if (!result.IsSuccess || string.IsNullOrEmpty(knyga?.virselio_nuotrauka))
            {
                return NotFound(new { message = "Knyga nerasta arba neturi viršelio nuotraukos" });
            }

            var deleted = _fileUploadService.DeleteImage(knyga.virselio_nuotrauka);
            if (!deleted)
            {
                return NotFound(new { message = "Nuotrauka nerasta" });
            }

            return NoContent();
        }
    }
}
