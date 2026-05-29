using auth_service.DTOs.Requests;
using auth_service.DTOs.Responses;
using auth_service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace auth_service.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    private Guid GetUserId()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        return Guid.Parse(userId!);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result =
            await _userService.GetProfileAsync(
                GetUserId()
            );

        return Ok(
            ApiResponse<UserInfoResponse>
                .SuccessResponse(
                    result!,
                    "Get profile success"
                )
        );
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        UpdateProfileRequest request)
    {
        var result =
            await _userService.UpdateProfileAsync(
                GetUserId(),
                request
            );

        return Ok(
            ApiResponse<UserInfoResponse>
                .SuccessResponse(
                    result!,
                    "Update profile success"
                )
        );
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequest request)
    {
        var result =
            await _userService.ChangePasswordAsync(
                GetUserId(),
                request
            );

        if (!result)
        {
            return BadRequest(
    ApiResponse<string>.FailResponse(
        "Logout failed",
        400
    )
);
        }

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null!,
                "Change password success"
            )
        );
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync(
            GetUserId()
        );

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null!,
                "Logout success"
            )
        );
    }
}