using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;

namespace PhysicalPersonDirectory.Api.ActionFilters;

public class ValidateParametersAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.Any())
        {
            base.OnActionExecuting(context);
            return;
        }

        var serviceProvider = context.HttpContext.RequestServices;

        foreach (var argument in context.ActionArguments)
        {
            var argumentType = argument.Value?.GetType();
            if (argumentType == null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var validator = serviceProvider.GetService(validatorType);

            if (validator == null) continue;
            if (argument.Value == null) return;
            var validationContext = new ValidationContext<object>(argument.Value);
            var validationResult = ((IValidator)validator).Validate(validationContext);

            if (validationResult.IsValid) continue;
            context.Result = new BadRequestObjectResult(validationResult.Errors);

            return;
        }

        base.OnActionExecuting(context);
    }
}