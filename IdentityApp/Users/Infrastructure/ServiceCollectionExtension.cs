using IdentityApp.Users.Infrastructure.Repositories;
using IdentityApp.Users.Infrastructure.Interfaces;

namespace IdentityApp.Users.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}
