namespace auth_service.DTOs.Requests;

public class UpdateProfileRequest
{
    public string FullName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? AvatarUrl { get; set; }
}