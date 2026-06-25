using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AllowedValuesAttribute : ValidationAttribute
{
    private readonly HashSet<string> _allowed;

    public AllowedValuesAttribute(params string[] allowedValues)
    {
        _allowed = new HashSet<string>(allowedValues ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is not string s)
        {
            return false;
        }

        return _allowed.Contains(s);
    }

    public override string FormatErrorMessage(string name)
        => $"{name} must be one of: {string.Join(", ", _allowed)}";
}
