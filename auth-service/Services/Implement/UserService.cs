using auth_service.Data;
using auth_service.DTOs.Requests;
using auth_service.DTOs.Responses;
using auth_service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Services.Implement;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserInfoResponse?> GetProfileAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return null;
        }

        return new UserInfoResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role.ToString(),
            AvatarUrl = user.AvatarUrl,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<UserInfoResponse?> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return null;
        }

        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;
        user.DateOfBirth = request.DateOfBirth;
        user.AvatarUrl = request.AvatarUrl;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new UserInfoResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role.ToString(),
            AvatarUrl = user.AvatarUrl,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<bool> ChangePasswordAsync(
        Guid userId,
        ChangePasswordRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return false;
        }

        var isCorrectPassword =
            BCrypt.Net.BCrypt.Verify(
                request.CurrentPassword,
                user.PasswordHash
            );

        if (!isCorrectPassword)
        {
            return false;
        }

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword
            );

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LogoutAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return false;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}