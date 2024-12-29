namespace IdentityApp.Users.CreateUser
{
    /// <summary>
    /// Represents the response for creating a user.
    /// </summary>
    public record CreateUserResponse(string login, string[] roles);
}

