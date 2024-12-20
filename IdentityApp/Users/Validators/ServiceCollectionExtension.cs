using IdentityApp.Users.Validators.RefreshTokenValidators;
using IdentityApp.Users.Validators.UserValidators;

namespace IdentityApp.Users.Validators
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {

            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IRefreshTokenValidator, RefreshTokenValidator>();

            return services;
        }
    }
}
