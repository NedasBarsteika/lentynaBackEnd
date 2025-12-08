using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Autoriai;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using lentynaBackEnd.DTOs.Citatos;


namespace lentynaBackEnd.Controllers
{
    [ApiController]
    [Route("api/autoriai")]
    
    public class AutoriaiController : ControllerBase
    {
        private readonly IAutoriusService _autoriusService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICitataService _citataService;

        public AutoriaiController(IAutoriusService autoriusService, ICitataService citataService, IFileUploadService fileUploadService)
        {
            _autoriusService = autoriusService;
            _citataService = citataService;
            _fileUploadService = fileUploadService;
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

        /// <summary>
        /// Įkelti autoriaus nuotrauką
        /// </summary>
        [HttpPost("{id}/nuotrauka")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> UploadAuthorPhoto(Guid id, IFormFile file)
        {
            // Validuoti ar autorius egzistuoja
            var (result, autorius) = await _autoriusService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(new { message = "Autorius nerastas" });
            }

            var (success, url, error) = await _fileUploadService.UploadImageAsync(file, "images/autoriai");

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { url });
        }

        /// <summary>
        /// Ištrinti autoriaus nuotrauką
        /// </summary>
        [HttpDelete("{id}/nuotrauka")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> DeleteAuthorPhoto(Guid id)
        {
            var (result, autorius) = await _autoriusService.GetByIdAsync(id);
            if (!result.IsSuccess || string.IsNullOrEmpty(autorius?.nuotrauka))
            {
                return NotFound(new { message = "Autorius nerastas arba neturi nuotraukos" });
            }

            var deleted = _fileUploadService.DeleteImage(autorius.nuotrauka);
            if (!deleted)
            {
                return NotFound(new { message = "Nuotrauka nerasta" });
        // [HttpGet("autorius/{autoriusId}")]
        // public async Task<IActionResult> GetByAutoriusId(Guid autoriusId)
        // {
        //     var citatos = await _citataService.GetByAutoriusIdAsync(autoriusId);
        //     return Ok(citatos);
        // }

        [HttpPost("citatos")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> Create([FromBody] CreateCitataDto dto)
        {
            var (result, citata) = await _citataService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(citata);
        }

        [HttpDelete("citatos/{id}")]
        [Authorize(Roles = "redaktorius,admin")]
        public async Task<IActionResult> DeleteCitata(Guid id)
        {
            var result = await _citataService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }
    }
}
