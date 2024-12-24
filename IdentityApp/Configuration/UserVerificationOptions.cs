using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Configuration
{
    public sealed class UserVerificationOptions
    {
        public static readonly string SectionName = "UserVerification";

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxLoginAttempts { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BlockUserExpirationTimeInMinutes { get; set; }

        [Required]
        public bool EnableUserManagementEndpoints { get; set; }
    }
}
