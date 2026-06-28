using Library.Application.DTOs.Auth;

namespace Library.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string? ipAddress, CancellationToken ct = default);
    Task<int> CreateUserAsync(CreateSystemUserDto request, CancellationToken ct = default);
}
