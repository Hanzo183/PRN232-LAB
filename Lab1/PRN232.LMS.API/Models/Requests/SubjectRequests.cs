using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Requests;

[DataContract(Name = "SubjectUpsertRequest", Namespace = "")]
public sealed class SubjectUpsertRequest
{
    [Required]
    [MaxLength(20)]
    [DataMember(Name = "subjectCode", Order = 1)]
    public string SubjectCode { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [DataMember(Name = "subjectName", Order = 2)]
    public string SubjectName { get; init; } = string.Empty;

    [Range(1, 10)]
    [DataMember(Name = "credit", Order = 3)]
    public int Credit { get; init; }
}
