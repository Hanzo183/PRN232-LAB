using FluentValidation;
using PRN232.LMS.API.Models.Requests;

namespace PRN232.LMS.API.Validators;

public sealed class StudentUpsertRequestValidator : AbstractValidator<StudentUpsertRequest>
{
    public StudentUpsertRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("DateOfBirth must be in the past.");
    }
}
