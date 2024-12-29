namespace IdentityApp.Users.RefreshUser
{
    /// <summary>
    /// Represents the response for logged user.
    /// </summary>
    public record RefreshUserResponse(string token, string refreshToken);
}