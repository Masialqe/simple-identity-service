using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Configuration
{
    public class PasswordOptions
    {
        public static readonly string SectionName = "PasswordPolicy";

        [Required]
        public int PasswordLength { get; set; }

        [Required] 
        public string? PasswordRegex { get; set; }

        [Required]
        public string? PasswordErrorMessage { get; set; }
    }
}
