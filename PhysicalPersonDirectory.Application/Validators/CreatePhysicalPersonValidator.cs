using FluentValidation;
using PhysicalPersonDirectory.Application.Commands;
using PhysicalPersonDirectory.Application.Shared.Constants;

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
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_FirstName);
        RuleFor(x => x.FirstName)
            .Matches(RegexConstants.NamesRegex)
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_FirstName_Regex);
        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_FirstName);
        RuleFor(x => x.LastName)
            .Matches(RegexConstants.NamesRegex)
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_FirstName_Regex);
        RuleFor(x => x.IdentificationNumber)
            .NotNull()
            .NotEmpty()
            .Length(11)
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_IdentificationNumber);
        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_LessThan18);
    }
}