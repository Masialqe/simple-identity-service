namespace IdentityApp.Common.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredOptions(this IServiceCollection services)
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<UserVerificationOptions>()
                .BindConfiguration(UserVerificationOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<SecretsOptions>()
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
