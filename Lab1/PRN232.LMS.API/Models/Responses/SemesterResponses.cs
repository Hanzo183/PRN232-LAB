using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "SemesterSummaryResponse", Namespace = "")]
public sealed record SemesterSummaryResponse(
    [property: DataMember(Name = "semesterId", Order = 1)] int SemesterId,
    [property: DataMember(Name = "semesterName", Order = 2)] string SemesterName,
    [property: DataMember(Name = "startDate", Order = 3)] DateTime StartDate,
    [property: DataMember(Name = "endDate", Order = 4)] DateTime EndDate);

[DataContract(Name = "SemesterResponse", Namespace = "")]
public sealed record SemesterResponse(
    [property: DataMember(Name = "semesterId", Order = 1)]
    int SemesterId,
    [property: DataMember(Name = "semesterName", Order = 2)]
    string SemesterName,
    [property: DataMember(Name = "startDate", Order = 3)]
    DateTime StartDate,
    [property: DataMember(Name = "endDate", Order = 4)]
    DateTime EndDate,
    [property: DataMember(Name = "courses", Order = 5)]
    List<CourseSummaryResponse>? Courses);
