using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.KnyguKlubas;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IKnyguKlubasService
    {
        Task<(Result Result, BalsavimasDto? Balsavimas)> GetCurrentAsync();
        Task<(Result Result, BalsavimasDto? Balsavimas)> GetByIdAsync(Guid id);
        Task<(Result Result, BalsavimasDto? Balsavimas)> CreateAsync(CreateBalsavimasDto dto);
        Task<(Result Result, bool Success)> VoteAsync(Guid naudotojasId, CreateBalsasDto dto);
        Task<Result> RemoveVoteAsync(Guid balsasId, Guid naudotojasId);
        Task<(Result Result, string? OroPrognoze)> GetOroPrognozeAsync(Guid balsavimasId);
        Task<(Result Result, Guid? VotedBookId)> GetUserVoteAsync(Guid balsavimasId, Guid naudotojasId);
    }
}
