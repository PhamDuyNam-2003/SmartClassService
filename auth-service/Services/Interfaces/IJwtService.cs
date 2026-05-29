using auth_service.Models;

namespace auth_service.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}