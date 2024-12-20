using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Common.Configuration
{
    public sealed class SecretsOptions
    {
        [Required]
        public string? SecretKey { get; set; }

        [Required]
        public string? ApiKey { get; set; }
    }
}
