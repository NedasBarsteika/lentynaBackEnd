using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileUploadService> _logger;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public FileUploadService(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<(bool Success, string? Url, string? Error)> UploadImageAsync(IFormFile file, string folder = "images")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return (false, null, "Failas nepateiktas");
                }

                if (!IsValidImage(file))
                {
                    return (false, null, $"Netinkamas failo formatas. Leidžiami: {string.Join(", ", _allowedExtensions)}");
                }

                if (file.Length > MaxFileSize)
                {
                    return (false, null, $"Failas per didelis. Maksimalus dydis: {MaxFileSize / 1024 / 1024} MB");
                }

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate URL
                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}";
                var imageUrl = $"{baseUrl}/{folder}/{uniqueFileName}";

                _logger.LogInformation("Image uploaded successfully: {Url}", imageUrl);

                return (true, imageUrl, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return (false, null, "Klaida įkeliant nuotrauką");
            }
        }

        public bool DeleteImage(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return false;

                // Extract filename from URL
                var uri = new Uri(imageUrl);
                var relativePath = uri.AbsolutePath.TrimStart('/');
                var filePath = Path.Combine(_environment.WebRootPath, relativePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Image deleted: {Path}", filePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image: {Url}", imageUrl);
                return false;
            }
        }

        public bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return false;

            // Check MIME type
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                return false;

            return true;
        }
    }
}
