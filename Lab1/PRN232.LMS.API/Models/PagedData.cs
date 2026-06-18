using System.Runtime.Serialization;

namespace PRN232.LMS.API.Models;

[DataContract(Name = "Pagination", Namespace = "")]
public sealed record Pagination(
    [property: DataMember(Name = "page", Order = 1)] int Page,
    [property: DataMember(Name = "pageSize", Order = 2)] int PageSize,
    [property: DataMember(Name = "totalItems", Order = 3)] int TotalItems,
    [property: DataMember(Name = "totalPages", Order = 4)] int TotalPages);

[DataContract(Name = "PagedData", Namespace = "")]
public sealed record PagedData<T>(
    [property: DataMember(Name = "items", Order = 1)] List<T> Items,
    [property: DataMember(Name = "pagination", Order = 2)] Pagination Pagination);
