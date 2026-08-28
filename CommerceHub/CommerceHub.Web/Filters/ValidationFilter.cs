using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommerceHub.Web.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                {
                    continue;
                }

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator is not null)
                {
                    var validationContext = new ValidationContext<object>(argument);
                   var result = await validator.ValidateAsync(validationContext);
                    if (!result.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(result.Errors.Select(e=>e.ErrorMessage));
                        return;
                    }

                }
            }
            await next();
        }
    }
}
