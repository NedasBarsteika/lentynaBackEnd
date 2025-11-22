using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Sekimai;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface ISekimasService
    {
        Task<IEnumerable<SekimasDto>> GetByNaudotojasIdAsync(Guid naudotojasId);
        Task<bool> IsFollowingAsync(Guid naudotojasId, Guid autoriusId);
        Task<(Result Result, SekimasDto? Sekimas)> FollowAsync(Guid naudotojasId, CreateSekimasDto dto);
        Task<Result> UnfollowAsync(Guid naudotojasId, Guid autoriusId);
    }
}
