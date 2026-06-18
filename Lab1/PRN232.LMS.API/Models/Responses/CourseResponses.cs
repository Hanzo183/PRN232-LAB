using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "CourseSummaryResponse", Namespace = "")]
public sealed record CourseSummaryResponse(
    [property: DataMember(Name = "courseId", Order = 1)] int CourseId,
    [property: DataMember(Name = "courseName", Order = 2)] string CourseName,
    [property: DataMember(Name = "semesterId", Order = 3)] int? SemesterId);

[DataContract(Name = "CourseResponse", Namespace = "")]
public sealed record CourseResponse(
    [property: DataMember(Name = "courseId", Order = 1)]
    int CourseId,
    [property: DataMember(Name = "courseName", Order = 2)]
    string CourseName,
    [property: DataMember(Name = "semesterId", Order = 3)]
    int? SemesterId,
    [property: DataMember(Name = "semester", Order = 4)]
    SemesterSummaryResponse? Semester,
    [property: DataMember(Name = "enrollments", Order = 5)]
    List<EnrollmentSummaryResponse>? Enrollments);
