using auth_service.Data;
using auth_service.DTOs.Requests;
using auth_service.DTOs.Responses;
using auth_service.Exceptions.Common;
using auth_service.Models;
using auth_service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Services.Implement;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    private readonly IJwtService _jwtService;

    public AuthService(
        AppDbContext context,
        IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string> RegisterAsync(
        RegisterRequest request)
    {
        request.Email =
            request.Email
                .Trim()
                .ToLower();

        request.UserName =
            request.UserName
                .Trim();

        request.FullName =
            request.FullName
                .Trim();

        var normalizedEmail =
            request.Email
                .ToUpperInvariant();

        var normalizedUserName =
            request.UserName
                .ToUpperInvariant();

        var emailExists =
            await _context.Users
                .AsNoTracking()
                .AnyAsync(x =>
                    !x.IsDeleted
                    &&
                    x.NormalizedEmail ==
                    normalizedEmail);

        if (emailExists)
        {
            throw new BadRequestException(
                "Email already exists"
            );
        }

        var usernameExists =
            await _context.Users
                .AsNoTracking()
                .AnyAsync(x =>
                    !x.IsDeleted
                    &&
                    x.NormalizedUserName ==
                    normalizedUserName);

        if (usernameExists)
        {
            throw new BadRequestException(
                "Username already exists"
            );
        }

        var user = new User
        {
            FullName =
                request.FullName,

            UserName =
                request.UserName,

            NormalizedUserName =
                normalizedUserName,

            Email =
                request.Email,

            NormalizedEmail =
                normalizedEmail,

            PasswordHash =
                BCrypt.Net.BCrypt
                    .HashPassword(
                        request.Password
                    ),

            Role =
                request.Role,

            CreatedAt =
                DateTime.UtcNow,

            UpdatedAt =
                DateTime.UtcNow
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return "Register success";
    }

    public async Task<AuthResponse?>
        LoginAsync(
            LoginRequest request)
    {
        var login =
            request.UserNameOrEmail
                .Trim()
                .ToUpperInvariant();

        var user =
            await _context.Users
                .FirstOrDefaultAsync(x =>
                    !x.IsDeleted
                    &&
                    (
                        x.NormalizedEmail ==
                        login
                        ||
                        x.NormalizedUserName ==
                        login
                    ));

        if (user == null)
        {
            return null;
        }

        var isPasswordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );

        if (!isPasswordValid)
        {
            return null;
        }

        var accessToken =
            _jwtService.GenerateToken(
                user);

        var refreshToken =
            _jwtService
                .GenerateRefreshToken();

        user.RefreshToken =
            refreshToken;

        user.RefreshTokenExpiryTime =
            DateTime.UtcNow
                .AddDays(7);

        user.UpdatedAt =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken =
                accessToken,

            RefreshToken =
                refreshToken,

            ExpiresIn =
                60 * 60 * 24 * 7,

            User =
                new UserInfoResponse
                {
                    Id = user.Id,

                    FullName =
                        user.FullName,

                    UserName =
                        user.UserName,

                    Email =
                        user.Email,

                    Role =
                        user.Role
                            .ToString(),

                    AvatarUrl =
                        user.AvatarUrl
                }
        };
    }

    public async Task<AuthResponse?>
        RefreshTokenAsync(
            string refreshToken)
    {
        var user =
            await _context.Users
                .FirstOrDefaultAsync(x =>
                    !x.IsDeleted
                    &&
                    x.RefreshToken ==
                    refreshToken);

        if (
            user == null
            ||
            user.RefreshTokenExpiryTime
                <= DateTime.UtcNow
        )
        {
            return null;
        }

        var newAccessToken =
            _jwtService.GenerateToken(
                user);

        var newRefreshToken =
            _jwtService
                .GenerateRefreshToken();

        user.RefreshToken =
            newRefreshToken;

        user.RefreshTokenExpiryTime =
            DateTime.UtcNow
                .AddDays(7);

        user.UpdatedAt =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken =
                newAccessToken,

            RefreshToken =
                newRefreshToken,

            ExpiresIn =
                60 * 60 * 24 * 7,

            User =
                new UserInfoResponse
                {
                    Id = user.Id,

                    FullName =
                        user.FullName,

                    UserName =
                        user.UserName,

                    Email =
                        user.Email,

                    Role =
                        user.Role
                            .ToString(),

                    AvatarUrl =
                        user.AvatarUrl
                }
        };
    }

    public async Task<bool>
        LogoutAsync(Guid userId)
    {
        var user =
            await _context.Users
                .FirstOrDefaultAsync(x =>
                    !x.IsDeleted
                    &&
                    x.Id == userId);

        if (user == null)
        {
            return false;
        }

        user.RefreshToken = null;

        user.RefreshTokenExpiryTime =
            null;

        user.UpdatedAt =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<UserProfileResponse?>
        GetProfileAsync(Guid userId)
    {
        var user =
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    !x.IsDeleted
                    &&
                    x.Id == userId);

        if (user == null)
        {
            return null;
        }

        return new UserProfileResponse
        {
            Id = user.Id,

            UserName =
                user.UserName,

            FullName =
                user.FullName,

            Email =
                user.Email,

            Role =
                user.Role
                    .ToString(),

            AvatarUrl =
                user.AvatarUrl,

            PhoneNumber =
                user.PhoneNumber
        };
    }
}