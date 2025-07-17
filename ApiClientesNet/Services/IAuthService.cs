using UserApi.DTOs;

namespace UserApi.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Register(AuthResponseDto authResponseDto);
    Task<AuthResponseDto> Login(LoginDto loginDto);
}
