using auth_service.DTOs.Requests;
using auth_service.DTOs.Responses;

namespace auth_service.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request);

    Task<AuthResponse?> LoginAsync(LoginRequest request);

    Task<AuthResponse?> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(Guid userId);

    Task<UserProfileResponse?> GetProfileAsync(Guid userId);






}