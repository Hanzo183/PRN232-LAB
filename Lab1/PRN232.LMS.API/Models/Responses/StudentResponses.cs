using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "StudentSummaryResponse", Namespace = "")]
public sealed record StudentSummaryResponse(
    [property: DataMember(Name = "studentId", Order = 1)] int StudentId,
    [property: DataMember(Name = "fullName", Order = 2)] string FullName,
    [property: DataMember(Name = "email", Order = 3)] string Email);

[DataContract(Name = "StudentResponse", Namespace = "")]
public sealed record StudentResponse(
    [property: DataMember(Name = "studentId", Order = 1)]
    int StudentId,
    [property: DataMember(Name = "fullName", Order = 2)]
    string FullName,
    [property: DataMember(Name = "email", Order = 3)]
    string Email,
    [property: DataMember(Name = "dateOfBirth", Order = 4)]
    DateTime DateOfBirth,
    [property: DataMember(Name = "enrollments", Order = 5)]
    List<EnrollmentSummaryResponse>? Enrollments);
