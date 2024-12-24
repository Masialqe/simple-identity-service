using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Configuration
{
    public sealed class JwtOptions
    {
        public static readonly string SectionName = "Jwt";

        [Required]
        public string? Issuer { get; set; }
        [Required]
        public string? Audience { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ExpirationInMinutes { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RefreshTokenExpireTimeInDays { get; set; }
    }
}
