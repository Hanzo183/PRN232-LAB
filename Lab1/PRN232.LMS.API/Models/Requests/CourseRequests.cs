using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Requests;

[DataContract(Name = "CourseUpsertRequest", Namespace = "")]
public sealed class CourseUpsertRequest
{
    [Required]
    [MaxLength(100)]
    [DataMember(Name = "courseName", Order = 1)]
    public string CourseName { get; init; } = string.Empty;

    [DataMember(Name = "semesterId", Order = 2)]
    public int? SemesterId { get; init; }
}
