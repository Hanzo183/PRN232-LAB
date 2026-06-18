using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models;

[DataContract(Name = "ApiError", Namespace = "")]
public sealed record ApiError(
    [property: DataMember(Name = "code", Order = 1)] string Code,
    [property: DataMember(Name = "message", Order = 2)] string Message);
