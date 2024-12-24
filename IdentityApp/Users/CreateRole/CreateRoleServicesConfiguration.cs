using IdentityApp.Users.CreateRole;

namespace IdentityApp.Users.CreateUser
{
    public static class CreateUserServicesConfiguration
    {
        public static IServiceCollection AddCreateRole(this IServiceCollection services)
        {
            services.AddScoped<ICreateRoleHandler, CreateRoleHandler>();

            return services;
        }
    }
}
