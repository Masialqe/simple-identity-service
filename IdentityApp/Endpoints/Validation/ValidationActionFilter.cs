using FluentValidation;
using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Common.Abstractions.Errors;

namespace IdentityApp.Endpoints.Validation
{
    public sealed class ValidationActionFilter<Trequest>(
        IValidator<Trequest> validator) : IEndpointFilter where Trequest : class
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.Arguments.OfType<Trequest>().FirstOrDefault();

            if (request is null)
                return GeneralErrors.EmptyRequestError;

            var validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
                return Result.Failure(GeneralErrors.ValidationError(validationResult.Errors)).ToProblemDetails();

            return await next(context);
        }
    }
}
