using FluentValidation;
using PhysicalPersonDirectory.Application.Commands;

namespace PhysicalPersonDirectory.Application.Validators;

public class CreatePhysicalPersonValidator : AbstractValidator<CreatePhysicalPersonCommand>
{
    public CreatePhysicalPersonValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage(Resources.CreatePhysicalPersonValidator_FirstName);
        RuleFor(x => x.FirstName)
            .Matches(@"^([a-zA-Z]+|[\u10A0-\u10FF]+)$")
            .WithMessage(Resources.CreatePhysicalPersonValidator_FirstName_Regex);
        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage(Resources.CreatePhysicalPersonValidator_FirstName);
        RuleFor(x => x.LastName)
            .Matches(@"^([a-zA-Z]+|[\u10A0-\u10FF]+)$")
            .WithMessage(Resources.CreatePhysicalPersonValidator_FirstName_Regex);
        RuleFor(x => x.IdentificationNumber)
            .NotNull()
            .NotEmpty()
            .Length(11)
            .WithMessage(Resources.CreatePhysicalPersonValidator_IdentificationNumber);
        ;
    }
}