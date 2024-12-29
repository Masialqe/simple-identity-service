namespace IdentityApp.Endpoints.Responses
{
    /// <summary>
    /// Represents a standardized API response.
    /// </summary>
    /// <param name="status">The HTTP status code of the response.</param>
    /// <param name="message">A message describing the outcome of the API call.</param>
    /// <param name="result">The data payload returned by the API call.</param>
    public sealed record ApiResponse(
        int status, string message, object result)
    {
        /// <summary>
        /// Creates a new <see cref="ApiResponse"/> instance representing a successful creation operation.
        /// </summary>
        /// <param name="status">The HTTP status code of the response.</param>
        /// <param name="message">A message describing the outcome of the creation operation.</param>
        /// <param name="result">The data payload returned by the creation operation, or an empty object if null.</param>
        /// <returns>A new <see cref="ApiResponse"/> instance with the specified status, message, and result.</returns>
        public static ApiResponse Created(int status, string message, object result)
            => new ApiResponse(status, message, result ?? new { });
    }

}
