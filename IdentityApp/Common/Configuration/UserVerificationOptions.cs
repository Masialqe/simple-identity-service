using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Common.Configuration
{
    public class UserVerificationOptions
    {
        public static string SectionName = "UserVerification";

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
