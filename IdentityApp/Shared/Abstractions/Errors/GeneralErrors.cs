using FluentValidation.Results;
using System.Text.Json;

namespace IdentityApp.Shared.Abstractions.Errors
{
    public static class GeneralErrors
    {
        public static Error ValidationError(List<ValidationFailure> errorList)
            => Error.BadRequest($"Request validatio failed. Error: {errorList.CollectionToJson()}");

        public static Error EmptyRequestError => Error.BadRequest("Invalid request payload.");
    }

    public static class GeneralErrorsExtensions
    {
        public static string CollectionToJson<T>(this IEnumerable<T> values)
        {
            return JsonSerializer.Serialize(values);
        }
    }
}
