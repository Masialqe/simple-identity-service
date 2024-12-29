using IdentityApp.Shared.Abstractions.Errors;

namespace IdentityApp.Shared.Abstractions.ApiResults
{
    /// <summary>
    /// Provides extension methods for converting <see cref="Result"/> and <see cref="Error"/> instances to <see cref="IResult"/> representing problem details.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Converts a <see cref="Result"/> to a <see cref="ProblemDetails"/> response.
        /// </summary>
        /// <param name="result">The result to convert. Must be a failure (<see cref="Result.IsSuccess"/> is <see langword="false"/>).</param>
        /// <returns>An <see cref="IResult"/> representing the problem details associated with the error.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the result is successful (<see cref="Result.IsSuccess"/> is <see langword="true"/>).</exception>
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot convert a successful result to problem details.");

            return Results.Problem(
                statusCode: result.Error.Code,
                title: result.Error.Type.ToString(),
                type: GetType(result.Error.Type),
                detail: result.Error.Description
                );

            /// <summary>
            /// Returns the corresponding URI for a given <see cref="ErrorType"/>.
            /// </summary>
            /// <param name="errorType">The error type to map to a URI.</param>
            /// <returns>A URI string that corresponds to the error type.</returns>
            static string GetType(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    ErrorType.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5",
                    ErrorType.Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                    ErrorType.Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                    _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
                };
        }

        /// <summary>
        /// Converts a <see cref="Error"/> to a <see cref="ProblemDetails"/> response by creating a <see cref="Result"/> from the error.
        /// </summary>
        /// <param name="error">The error to convert.</param>
        /// <returns>An <see cref="IResult"/> representing the problem details associated with the error.</returns>
        public static IResult ToProblemDetails(this Error error)
        {
            var errorAsResult = (Result)error;

            return errorAsResult.ToProblemDetails();
        }
    }
}
