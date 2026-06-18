using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "EnrollmentResponse", Namespace = "")]
public sealed record EnrollmentResponse(
    [property: DataMember(Name = "enrollmentId", Order = 1)]
    int EnrollmentId,
    [property: DataMember(Name = "studentId", Order = 2)]
    int? StudentId,
    [property: DataMember(Name = "courseId", Order = 3)]
    int? CourseId,
    [property: DataMember(Name = "enrollDate", Order = 4)]
    DateTime? EnrollDate,
    [property: DataMember(Name = "status", Order = 5)]
    string Status,
    [property: DataMember(Name = "student", Order = 6)]
    StudentSummaryResponse? Student,
    [property: DataMember(Name = "course", Order = 7)]
    CourseSummaryResponse? Course);

[DataContract(Name = "EnrollmentSummaryResponse", Namespace = "")]
public sealed record EnrollmentSummaryResponse(
    [property: DataMember(Name = "enrollmentId", Order = 1)]
    int EnrollmentId,
    [property: DataMember(Name = "enrollDate", Order = 2)]
    DateTime? EnrollDate,
    [property: DataMember(Name = "status", Order = 3)]
    string Status,
    [property: DataMember(Name = "student", Order = 4)]
    StudentSummaryResponse? Student,
    [property: DataMember(Name = "course", Order = 5)]
    CourseSummaryResponse? Course);
