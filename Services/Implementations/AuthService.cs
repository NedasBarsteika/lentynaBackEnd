using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Auth;
using lentynaBackEnd.Helpers;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly INaudotojasRepository _naudotojasRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(INaudotojasRepository naudotojasRepository, JwtHelper jwtHelper)
        {
            _naudotojasRepository = naudotojasRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<(Result Result, AuthResponseDto? Response)> RegisterAsync(RegisterDto dto)
        {
            if (await _naudotojasRepository.ExistsByEmailAsync(dto.el_pastas))
            {
                return (Result.Failure(Constants.NaudotojasJauEgzistuoja), null);
            }

            if (await _naudotojasRepository.ExistsBySlapyvardisAsync(dto.slapyvardis))
            {
                return (Result.Failure(Constants.SlapyvardisJauEgzistuoja), null);
            }

            var naudotojas = new Naudotojas
            {
                slapyvardis = dto.slapyvardis,
                el_pastas = dto.el_pastas,
                slaptazodis = PasswordHelper.HashPassword(dto.slaptazodis),
                role = Roles.naudotojas
            };

            await _naudotojasRepository.AddAsync(naudotojas);

            var token = _jwtHelper.GenerateToken(naudotojas);

            var response = new AuthResponseDto
            {
                token = token,
                naudotojas = MapToDto(naudotojas)
            };

            return (Result.Success(), response);
        }

        public async Task<(Result Result, AuthResponseDto? Response)> LoginAsync(LoginDto dto)
        {
            var naudotojas = await _naudotojasRepository.GetByEmailAsync(dto.el_pastas);

            if (naudotojas == null)
            {
                return (Result.Failure(Constants.NaudotojasNerastas), null);
            }

            if (!PasswordHelper.VerifyPassword(dto.slaptazodis, naudotojas.slaptazodis))
            {
                return (Result.Failure(Constants.NeteisingasSlaptazodis), null);
            }

            var token = _jwtHelper.GenerateToken(naudotojas);

            var response = new AuthResponseDto
            {
                token = token,
                naudotojas = MapToDto(naudotojas)
            };

            return (Result.Success(), response);
        }

        public async Task<(Result Result, NaudotojasDto? Profile)> GetProfileAsync(Guid naudotojasId)
        {
            var naudotojas = await _naudotojasRepository.GetByIdAsync(naudotojasId);

            if (naudotojas == null)
            {
                return (Result.Failure(Constants.NaudotojasNerastas), null);
            }

            return (Result.Success(), MapToDto(naudotojas));
        }

        public async Task<(Result Result, NaudotojasDto? Profile)> UpdateProfileAsync(Guid naudotojasId, UpdateProfileDto dto)
        {
            var naudotojas = await _naudotojasRepository.GetByIdAsync(naudotojasId);

            if (naudotojas == null)
            {
                return (Result.Failure(Constants.NaudotojasNerastas), null);
            }

            if (!string.IsNullOrEmpty(dto.slapyvardis) && dto.slapyvardis != naudotojas.slapyvardis)
            {
                if (await _naudotojasRepository.ExistsBySlapyvardisAsync(dto.slapyvardis))
                {
                    return (Result.Failure(Constants.SlapyvardisJauEgzistuoja), null);
                }
                naudotojas.slapyvardis = dto.slapyvardis;
            }

            if (dto.profilio_nuotrauka != null)
            {
                naudotojas.profilio_nuotrauka = dto.profilio_nuotrauka;
            }

            await _naudotojasRepository.UpdateAsync(naudotojas);

            return (Result.Success(), MapToDto(naudotojas));
        }

        public async Task<Result> DeleteAccountAsync(Guid naudotojasId)
        {
            var success = await _naudotojasRepository.DeleteAsync(naudotojasId);

            if (!success)
            {
                return Result.Failure(Constants.NaudotojasNerastas);
            }

            return Result.Success();
        }

        public async Task<Result> UpdateRoleAsync(Guid naudotojasId, Roles newRole)
        {
            var naudotojas = await _naudotojasRepository.GetByIdAsync(naudotojasId);

            if (naudotojas == null)
            {
                return Result.Failure(Constants.NaudotojasNerastas);
            }

            naudotojas.role = newRole;
            await _naudotojasRepository.UpdateAsync(naudotojas);

            return Result.Success();
        }

        private static NaudotojasDto MapToDto(Naudotojas naudotojas)
        {
            return new NaudotojasDto
            {
                Id = naudotojas.Id,
                slapyvardis = naudotojas.slapyvardis,
                el_pastas = naudotojas.el_pastas,
                role = naudotojas.role.ToString(),
                sukurimo_data = naudotojas.sukurimo_data,
                profilio_nuotrauka = naudotojas.profilio_nuotrauka
            };
        }
    }
}
