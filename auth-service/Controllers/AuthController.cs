using auth_service.DTOs.Requests;
using auth_service.DTOs.Responses;
using auth_service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace auth_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                result,
                "Register success"
            )
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return BadRequest(
     ApiResponse<string>.FailResponse(
         "Logout failed",
         400
     )
 );
        }

        return Ok(
            ApiResponse<AuthResponse>.SuccessResponse(
                result,
                "Login success"
            )
        );
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    RefreshTokenRequest request)
    {
        var result = await _authService
            .RefreshTokenAsync(request.RefreshToken);

        if (result == null)
        {
            return BadRequest(
    ApiResponse<string>.FailResponse(
        "Logout failed",
        400
    )
);
        }

        return Ok(
            ApiResponse<AuthResponse>.SuccessResponse(
                result,
                "Refresh token success"
            )
        );
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier
        )?.Value;

        if (userIdClaim == null)
        {
            return BadRequest(
     ApiResponse<string>.FailResponse(
         "Logout failed",
         400
     )
 );
        }

        var result = await _authService
            .LogoutAsync(Guid.Parse(userIdClaim));

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
                "",
                "Logout success"
            )
        );
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier
        )?.Value;

        if (userIdClaim == null)
        {
            return BadRequest(
    ApiResponse<string>.FailResponse(
        "Logout failed",
        400
    )
);
        }

        var result = await _authService
            .GetProfileAsync(Guid.Parse(userIdClaim));

        if (result == null)
        {
            return BadRequest(
    ApiResponse<string>.FailResponse(
        "Logout failed",
        400
    )
);
        }

        return Ok(
            ApiResponse<UserProfileResponse>
                .SuccessResponse(
                    result,
                    "Get profile success"
                )
        );
    }




}