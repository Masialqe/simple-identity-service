using IdentityApp.Shared.Abstractions.ApiResults;

namespace IdentityApp.Shared.Abstractions.Errors
{
    public record Error
    {
        public static readonly Error None = new(StatusCodes.Status500InternalServerError, string.Empty, ErrorType.Failure);

        public static implicit operator Result(Error error)
            => Result.Failure(error);

        private Error(int code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public int Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        public static Error BadRequest(string description)
            => new(StatusCodes.Status400BadRequest, description, ErrorType.BadRequest);

        public static Error Unauthorized(string description)
            => new(StatusCodes.Status401Unauthorized, description, ErrorType.Unauthorized);

        public static Error Conflict(string description)
            => new(StatusCodes.Status409Conflict, description, ErrorType.Conflict);

        public static Error Failure(string description)
            => new(StatusCodes.Status500InternalServerError, description, ErrorType.Failure);

        public static Error Forbidden(string description)
            => new(StatusCodes.Status403Forbidden, description, ErrorType.Forbidden);

    }

    public enum ErrorType
    {
        BadRequest = 0,
        Unauthorized = 1,
        Conflict = 2,
        Failure = 3,
        Forbidden = 4,
    }
}
