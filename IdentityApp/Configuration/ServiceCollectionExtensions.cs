using Microsoft.Extensions.Options;

namespace IdentityApp.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredOptions(this IServiceCollection services)
        {
            services.AddCongifuredOptions<JwtOptions>(JwtOptions.SectionName);
            services.AddCongifuredOptions<UserVerificationOptions>(UserVerificationOptions.SectionName);
            services.AddCongifuredOptions<SecretsOptions>(SecretsOptions.SectionName);
            services.AddCongifuredOptions<PasswordOptions>(PasswordOptions.SectionName);

            return services;
        }

        private static OptionsBuilder<TValue> AddCongifuredOptions<TValue>(this IServiceCollection services,
            string sectionName) where TValue : class
        {
            return services.AddOptions<TValue>()
                .BindConfiguration(sectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
