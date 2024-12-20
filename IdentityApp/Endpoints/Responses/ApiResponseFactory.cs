namespace IdentityApp.Endpoints.Responses
{
    public static class ApiResponseFactory
    {
        public static IResult Created(string location, object result)
            => Results.Created(
                location,
                ApiResponse.Create(StatusCodes.Status201Created, "Item has been created.", result));

        public static IResult Ok(object result)
            => Results.Ok(
                ApiResponse.Create(StatusCodes.Status200OK, "Request executed succesfully.", result));
    }
}
