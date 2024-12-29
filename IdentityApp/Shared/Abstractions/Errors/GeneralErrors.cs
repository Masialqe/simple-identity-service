using FluentValidation.Results;
using System.Text.Json;

namespace IdentityApp.Shared.Abstractions.Errors
{
    /// <summary>
    /// A collection of general error methods to represent common validation and request errors.
    /// </summary>
    public static class GeneralErrors
    {
        /// <summary>
        /// Creates a validation error with a detailed list of validation failures.
        /// </summary>
        /// <param name="errorList">A list of validation failures to include in the error message.</param>
        /// <returns>A new <see cref="Error"/> representing a validation error with the list of failures serialized to JSON.</returns>
        public static Error ValidationError(List<ValidationFailure> errorList)
            => Error.BadRequest($"Request validation failed. Error: {errorList.CollectionToJson()}");

        /// <summary>
        /// Creates a generic "Empty Request" error indicating an invalid request payload.
        /// </summary>
        /// <value>A new <see cref="Error"/> representing an empty or invalid request error.</value>
        public static Error EmptyRequestError => Error.BadRequest("Invalid request payload.");
    }

    /// <summary>
    /// Extension methods for collections, specifically for serializing them into JSON format.
    /// </summary>
    public static class GeneralErrorsExtensions
    {
        /// <summary>
        /// Serializes an enumerable collection into a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="values">The collection to be serialized.</param>
        /// <returns>A JSON-formatted string representing the collection.</returns>
        public static string CollectionToJson<T>(this IEnumerable<T> values)
        {
            return JsonSerializer.Serialize(values);
        }
    }

}
