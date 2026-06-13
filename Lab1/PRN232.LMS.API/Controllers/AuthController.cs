using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[ApiVersion(1.0)]
[ApiVersion(2.0)]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthTokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthTokenResponse>>> Login(
        [FromBody] LoginRequest request,
        [FromHeader(Name = "X-Request-Id")] string? requestId)
    {
        var result = await _auth.LoginAsync(request.Username, request.Password);
        if (!result.Success)
        {
            return result.Error!.Code == "Unauthorized"
                ? Unauthorized(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message))
                : BadRequest(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message));
        }

        var dto = new AuthTokenResponse(result.Data!.AccessToken, result.Data.RefreshToken, result.Data.ExpiresIn);
        return Ok(ApiResponse<AuthTokenResponse>.Ok(dto));
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<AuthTokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthTokenResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _auth.RefreshTokenAsync(request.RefreshToken);
        if (!result.Success)
        {
            return result.Error!.Code == "Unauthorized"
                ? Unauthorized(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message))
                : BadRequest(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message));
        }

        var dto = new AuthTokenResponse(result.Data!.AccessToken, result.Data.RefreshToken, result.Data.ExpiresIn);
        return Ok(ApiResponse<AuthTokenResponse>.Ok(dto));
    }
}
