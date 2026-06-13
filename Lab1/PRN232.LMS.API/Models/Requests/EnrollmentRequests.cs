using System.ComponentModel.DataAnnotations;
using PRN232.LMS.API.Validation;

namespace PRN232.LMS.API.Models.Requests;

public sealed class EnrollmentUpsertRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int StudentId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CourseId { get; init; }

    public DateTime? EnrollDate { get; init; }

    [Required]
    [MaxLength(20)]
    [PRN232.LMS.API.Validation.AllowedValues("active", "completed", "dropped")]
    public string Status { get; init; } = string.Empty;
}
