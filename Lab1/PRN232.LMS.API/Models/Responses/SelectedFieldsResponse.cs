using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models.Responses;

[DataContract(Name = "SelectedFieldValue", Namespace = "")]
public sealed record SelectedFieldValue(
    [property: DataMember(Name = "name", Order = 1)] string Name,
    [property: DataMember(Name = "value", Order = 2)] string? Value);

[DataContract(Name = "SelectedFieldsResponse", Namespace = "")]
public sealed record SelectedFieldsResponse(
    [property: DataMember(Name = "fields", Order = 1)] List<SelectedFieldValue> Fields);
