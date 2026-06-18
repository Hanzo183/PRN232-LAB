using System.Reflection;
using PRN232.LMS.API.Models.Responses;

namespace PRN232.LMS.API.Infrastructure;

public static class FieldSelection
{
    public static (bool Success, string? Error, List<SelectedFieldsResponse> Items) Shape<T>(
        IReadOnlyList<T> items,
        IReadOnlyList<string>? fields)
        where T : class
    {
        if (fields is null || fields.Count == 0)
        {
            return (true, null, new List<SelectedFieldsResponse>());
        }

        var props = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        foreach (var f in fields)
        {
            if (!props.ContainsKey(f))
            {
                return (false, $"Invalid field '{f}'.", new List<SelectedFieldsResponse>());
            }
        }

        var shaped = new List<SelectedFieldsResponse>(items.Count);
        foreach (var item in items)
        {
            var selectedFields = new List<SelectedFieldValue>(fields.Count);
            foreach (var f in fields)
            {
                var prop = props[f];
                var value = prop.GetValue(item);
                selectedFields.Add(new SelectedFieldValue(prop.Name, FormatValue(value)));
            }
            shaped.Add(new SelectedFieldsResponse(selectedFields));
        }

        return (true, null, shaped);
    }

    private static string? FormatValue(object? value)
        => value switch
        {
            null => null,
            DateTime dateTime => dateTime.ToString("O"),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("O"),
            _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture),
        };
}
