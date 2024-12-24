using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Infrastructure.Repositories;

namespace IdentityApp.Shared.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}
