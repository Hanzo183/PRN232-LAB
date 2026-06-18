using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Requests;

[DataContract(Name = "StudentUpsertRequest", Namespace = "")]
public sealed class StudentUpsertRequest
{
    [Required]
    [MaxLength(100)]
    [DataMember(Name = "fullName", Order = 1)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    [DataMember(Name = "email", Order = 2)]
    public string Email { get; init; } = string.Empty;

    [Phone]
    [DataMember(Name = "phone", Order = 3)]
    public string? Phone { get; init; }

    [RegularExpression(@"^[A-Z]{2}\d{5}$", ErrorMessage = "StudentCode must be FPTU style, e.g. SE19886")]
    [DataMember(Name = "studentCode", Order = 4)]
    public string? StudentCode { get; init; }

    [DataMember(Name = "dateOfBirth", Order = 5)]
    public DateTime DateOfBirth { get; init; }
}
