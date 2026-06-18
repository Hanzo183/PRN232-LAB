using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "AuthTokenResponse", Namespace = "")]
public sealed record AuthTokenResponse(
    [property: DataMember(Name = "accessToken", Order = 1)] string AccessToken,
    [property: DataMember(Name = "refreshToken", Order = 2)] string RefreshToken,
    [property: DataMember(Name = "expiresIn", Order = 3)] int ExpiresIn);
