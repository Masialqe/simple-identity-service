﻿using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Common.Abstractions.Errors;
using FluentValidation;

namespace IdentityApp.Endpoints.Validation
{
    public sealed class ValidationActionFilter<TRequest>(
        IValidator<TRequest> validator) : IEndpointFilter where TRequest : class
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

            if (request is null)
                return GeneralErrors.EmptyRequestError;

            var validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
                return Result.Failure(GeneralErrors.ValidationError(validationResult.Errors)).ToProblemDetails();

            return await next(context);
        }
    }
}
