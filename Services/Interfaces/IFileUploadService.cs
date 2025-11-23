namespace lentynaBackEnd.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<(bool Success, string? Url, string? Error)> UploadImageAsync(IFormFile file, string folder = "images");
        bool DeleteImage(string imageUrl);
        bool IsValidImage(IFormFile file);
    }
}
