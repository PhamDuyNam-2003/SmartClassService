using auth_service.Models.Common;
using auth_service.Models.Enums;

namespace auth_service.Models;

public class User : BaseEntity
{
    public string UserName { get; set; }
        = string.Empty;

    public string NormalizedUserName { get; set; }
        = string.Empty;

    public string FullName { get; set; }
        = string.Empty;

    public string Email { get; set; }
        = string.Empty;

    public string NormalizedEmail { get; set; }
        = string.Empty;

    public string PasswordHash { get; set; }
        = string.Empty;

    public UserRole Role { get; set; }
        = UserRole.Student;

    public string? AvatarUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }
        = Gender.Other;

    public string? Address { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime
    { get; set; }


}