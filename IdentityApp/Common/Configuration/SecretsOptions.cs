﻿using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Common.Configuration
{
    public sealed class SecretsOptions
    {
        public static readonly string SectionName = "Secrets";
        
        [Required]
        public string? SecretKey { get; set; }

        [Required]
        public string? ApiKey { get; set; }
    }
}
