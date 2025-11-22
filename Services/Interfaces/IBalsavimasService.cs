using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Balsavimai;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IBalsavimasService
    {
        Task<(Result Result, BalsavimasDto? Balsavimas)> GetCurrentAsync();
        Task<(Result Result, BalsavimasDto? Balsavimas)> GetByIdAsync(Guid id);
        Task<(Result Result, BalsavimasDto? Balsavimas)> CreateAsync(CreateBalsavimasDto dto);
        Task<(Result Result, bool Success)> VoteAsync(Guid naudotojasId, CreateBalsasDto dto);
        Task<Result> RemoveVoteAsync(Guid balsasId, Guid naudotojasId);
        Task<(Result Result, string? OroPrognoze)> GetOroPrognozeAsync(Guid balsavimasId);
    }
}
