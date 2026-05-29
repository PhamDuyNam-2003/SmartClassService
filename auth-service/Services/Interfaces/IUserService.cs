using auth_service.DTOs.Requests;
using auth_service.DTOs.Responses;

namespace auth_service.Services.Interfaces;

public interface IUserService
{
    Task<UserInfoResponse?> GetProfileAsync(Guid userId);

    Task<UserInfoResponse?> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequest request
    );

    Task<bool> ChangePasswordAsync(
        Guid userId,
        ChangePasswordRequest request
    );

    Task<bool> LogoutAsync(Guid userId);
}