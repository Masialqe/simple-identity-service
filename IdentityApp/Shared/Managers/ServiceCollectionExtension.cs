using IdentityApp.Shared.Managers.Interrfaces;

namespace IdentityApp.Shared.Managers
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IJwtManager, JwtManager>();
            services.AddScoped<IRefreshTokenManager, RefreshTokenManager>();
            services.AddScoped<ITokenManager, TokenManager>();

            return services;
        }
    }
}
