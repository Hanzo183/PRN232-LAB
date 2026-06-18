using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Requests;

[DataContract(Name = "LoginRequest", Namespace = "")]
public sealed class LoginRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [DataMember(Name = "username", Order = 1)]
    public string Username { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataMember(Name = "password", Order = 2)]
    public string Password { get; init; } = string.Empty;
}

[DataContract(Name = "RefreshTokenRequest", Namespace = "")]
public sealed class RefreshTokenRequest
{
    [Required]
    [DataMember(Name = "refreshToken", Order = 1)]
    public string RefreshToken { get; init; } = string.Empty;
}
