using FluentValidation;
using PhysicalPersonDirectory.Application.Models;

namespace PhysicalPersonDirectory.Application.Validators;

public class PhoneNumberValidator : AbstractValidator<PhoneNumberModel>
{
    public PhoneNumberValidator()
    {
        RuleFor(x => x.Number)
            .NotNull()
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(50)
            .WithMessage(Resources.PhoneNumberValidator_PhoneNumber);
        ;
    }
}