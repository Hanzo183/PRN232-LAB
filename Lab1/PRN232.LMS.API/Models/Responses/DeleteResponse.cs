using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "DeleteResponse", Namespace = "")]
public sealed record DeleteResponse(
    [property: DataMember(Name = "deleted", Order = 1)] bool Deleted);
