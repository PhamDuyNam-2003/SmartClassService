namespace auth_service.DTOs.Responses;

public class UserProfileResponse
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }
}