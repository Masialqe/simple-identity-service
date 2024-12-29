namespace IdentityApp.Shared.Domain.Models
{
    /// <summary>
    /// Represents a refresh token used for renewing authentication sessions.
    /// </summary>
    public sealed class RefreshToken
    {
        public static RefreshToken Create(string token, User user, DateTime expireOnUtc)
            => new RefreshToken { UserId = user.Id, Token = token, ExpiresOnUtc = expireOnUtc };

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public User? User { get; set; }
    }

}
