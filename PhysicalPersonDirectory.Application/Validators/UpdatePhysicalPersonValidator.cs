using FluentValidation;
using PhysicalPersonDirectory.Application.Commands;

namespace PhysicalPersonDirectory.Application.Validators;

public class UpdatePhysicalPersonValidator : AbstractValidator<UpdatePhysicalPersonCommand>
{
    public UpdatePhysicalPersonValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage(Resources.Resources.UpdatePhysicalPersonValidator_FirstName);
        RuleFor(x => x.FirstName)
            .Matches(@"^([a-zA-Z]+|[\u10A0-\u10FF]+)$")
            .WithMessage(Resources.Resources.UpdatePhysicalPersonValidator_FirstName_Regex);
        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage(Resources.Resources.UpdatePhysicalPersonValidator_FirstName);
        RuleFor(x => x.LastName)
            .Matches(@"^([a-zA-Z]+|[\u10A0-\u10FF]+)$")
            .WithMessage(Resources.Resources.UpdatePhysicalPersonValidator_FirstName_Regex);
        RuleFor(x => x.IdentificationNumber)
            .NotNull()
            .NotEmpty()
            .Length(11)
            .WithMessage(Resources.Resources.UpdatePhysicalPersonValidator_IdentificationNumber);
        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
            .WithMessage(Resources.Resources.CreatePhysicalPersonValidator_LessThan18);
        ;
    }
}