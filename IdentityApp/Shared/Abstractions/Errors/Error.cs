using IdentityApp.Shared.Abstractions.ApiResults;

namespace IdentityApp.Shared.Abstractions.Errors
{
    /// <summary>
    /// Represents an error with an associated HTTP status code, description, and type.
    /// </summary>
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

        /// <summary>
        /// Gets the HTTP status code associated with the error.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Gets the description of the error.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the type of the error.
        /// </summary>
        public ErrorType Type { get; }

        /// <summary>
        /// Creates a "Bad Request" error with the specified description.
        /// </summary>
        /// <param name="description">The description of the bad request error.</param>
        /// <returns>A new <see cref="Error"/> representing a Bad Request.</returns>
        public static Error BadRequest(string description)
            => new(StatusCodes.Status400BadRequest, description, ErrorType.BadRequest);

        /// <summary>
        /// Creates an "Unauthorized" error with the specified description.
        /// </summary>
        /// <param name="description">The description of the unauthorized error.</param>
        /// <returns>A new <see cref="Error"/> representing Unauthorized.</returns>
        public static Error Unauthorized(string description)
            => new(StatusCodes.Status401Unauthorized, description, ErrorType.Unauthorized);

        /// <summary>
        /// Creates a "Conflict" error with the specified description.
        /// </summary>
        /// <param name="description">The description of the conflict error.</param>
        /// <returns>A new <see cref="Error"/> representing Conflict.</returns>
        public static Error Conflict(string description)
            => new(StatusCodes.Status409Conflict, description, ErrorType.Conflict);

        /// <summary>
        /// Creates a generic "Failure" error with the specified description.
        /// </summary>
        /// <param name="description">The description of the failure error.</param>
        /// <returns>A new <see cref="Error"/> representing a generic failure.</returns>
        public static Error Failure(string description)
            => new(StatusCodes.Status500InternalServerError, description, ErrorType.Failure);

        /// <summary>
        /// Creates a "Forbidden" error with the specified description.
        /// </summary>
        /// <param name="description">The description of the forbidden error.</param>
        /// <returns>A new <see cref="Error"/> representing Forbidden.</returns>
        public static Error Forbidden(string description)
            => new(StatusCodes.Status403Forbidden, description, ErrorType.Forbidden);

    }

    /// <summary>
    /// Represents the type of error that occurred, corresponding to different HTTP status codes.
    /// </summary>
    public enum ErrorType
    {
        BadRequest = 0,
        Unauthorized = 1,
        Conflict = 2,
        Failure = 3,
        Forbidden = 4,
    }
}
