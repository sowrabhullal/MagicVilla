using MagicVillaWebApi.Models;
using MagicVillaWebApi.Models.Dto;

namespace MagicVillaWebApi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUnique(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto);
    }
}
