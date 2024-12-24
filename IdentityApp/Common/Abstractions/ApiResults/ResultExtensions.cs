using IdentityApp.Common.Abstractions.Errors;

namespace IdentityApp.Common.Abstractions.ApiResults
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException();

            return Results.Problem(
                statusCode: result.Error.Code,
                title: result.Error.Type.ToString(),
                type: GetType(result.Error.Type),
                detail: result.Error.Description
                );

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

        public static IResult ToProblemDetails(this Error error)
        {
            var errorAsResult = (Result) error;

            return errorAsResult.ToProblemDetails();
        }
    }
}
