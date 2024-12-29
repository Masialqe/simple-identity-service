namespace IdentityApp.Users.LoginUser
{
    /// <summary>
    /// Represents the response for logged user.
    /// </summary>
    public record LoginUserResponse(string token, string refreshToken);
}
