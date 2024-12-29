namespace IdentityApp.Endpoints.Responses
{
    public static class ApiResponseFactory
    {
        /// <summary>
        /// Returns a Created (201) response with the specified location and result.
        /// </summary>
        /// <param name="location">The location URI of the created resource.</param>
        /// <param name="result">The result object to include in the response.</param>
        /// <returns>An HTTP 201 Created result.</returns>
        public static IResult Created(string location, object result)
            => Results.Created(
                location,
                ApiResponse.Created(StatusCodes.Status201Created, "Item has been created.", result));

        /// <summary>
        /// Returns an Ok (200) response with the specified result.
        /// </summary>
        /// <param name="result">The result object to include in the response.</param>
        /// <returns>An HTTP 200 Ok result.</returns>
        public static IResult Ok(object result)
            => Results.Ok(
                ApiResponse.Created(StatusCodes.Status200OK, "Request executed successfully.", result));
    }
}
