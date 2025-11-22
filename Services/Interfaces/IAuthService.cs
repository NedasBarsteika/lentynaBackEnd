using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Auth;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(Result Result, AuthResponseDto? Response)> RegisterAsync(RegisterDto dto);
        Task<(Result Result, AuthResponseDto? Response)> LoginAsync(LoginDto dto);
        Task<(Result Result, NaudotojasDto? Profile)> GetProfileAsync(Guid naudotojasId);
        Task<(Result Result, NaudotojasDto? Profile)> UpdateProfileAsync(Guid naudotojasId, UpdateProfileDto dto);
        Task<Result> DeleteAccountAsync(Guid naudotojasId);
        Task<Result> UpdateRoleAsync(Guid naudotojasId, Roles newRole);
    }
}
