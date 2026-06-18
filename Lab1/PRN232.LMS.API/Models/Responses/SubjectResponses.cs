using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "SubjectResponse", Namespace = "")]
public sealed record SubjectResponse(
    [property: DataMember(Name = "subjectId", Order = 1)] int SubjectId,
    [property: DataMember(Name = "subjectCode", Order = 2)] string SubjectCode,
    [property: DataMember(Name = "subjectName", Order = 3)] string SubjectName,
    [property: DataMember(Name = "credit", Order = 4)] int Credit);
