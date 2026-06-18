using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Requests;

[DataContract(Name = "SemesterUpsertRequest", Namespace = "")]
public sealed class SemesterUpsertRequest
{
    [Required]
    [MaxLength(100)]
    [DataMember(Name = "semesterName", Order = 1)]
    public string SemesterName { get; init; } = string.Empty;

    [DataMember(Name = "startDate", Order = 2)]
    public DateTime StartDate { get; init; }

    [DataMember(Name = "endDate", Order = 3)]
    public DateTime EndDate { get; init; }
}
