namespace IdentityApp.Endpoints.Responses
{
    public sealed record ApiResponse(
        int status, string message, object result)
    {
        public static ApiResponse Create(int status, string message, object result)
            => new ApiResponse(status, message, result ?? new {});
    }
}
