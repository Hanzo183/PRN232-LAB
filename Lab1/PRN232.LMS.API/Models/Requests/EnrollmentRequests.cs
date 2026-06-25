using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Requests;

[DataContract(Name = "EnrollmentUpsertRequest", Namespace = "")]
public sealed class EnrollmentUpsertRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    [DataMember(Name = "studentId", Order = 1)]
    public int StudentId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    [DataMember(Name = "courseId", Order = 2)]
    public int CourseId { get; init; }

    [DataMember(Name = "enrollDate", Order = 3)]
    public DateTime? EnrollDate { get; init; }

    [Required]
    [MaxLength(20)]
    [PRN232.LMS.API.Validators.AllowedValues("active", "completed", "dropped")]
    [DataMember(Name = "status", Order = 4)]
    public string Status { get; init; } = string.Empty;
}
