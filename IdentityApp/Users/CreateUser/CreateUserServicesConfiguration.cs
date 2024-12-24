using IdentityApp.Users.CreateUser;

namespace IdentityApp.Users.CreateRole
{
    public static class CreateUserServicesConfiguration
    {
        public static IServiceCollection AddCreateUser(this IServiceCollection services)
        {
            services.AddScoped<ICreateUserHandler, CreateUserHandler>();

            return services;
        }
    }
}
