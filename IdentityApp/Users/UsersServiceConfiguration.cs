using IdentityApp.Users.CreateRole;
using IdentityApp.Users.CreateUser;
using IdentityApp.Users.LoginUser;
using IdentityApp.Users.RefreshUser;

namespace IdentityApp.Users
{
    /// <summary>
    /// Adds all User related features to DI container.
    /// </summary>
    public static class UsersServiceConfiguration
    {
        public static IServiceCollection AddUserFeatures(this IServiceCollection services)
        {
            services.AddCreateRole()
                    .AddCreateUser()
                    .AddLoginUser()
                    .AddRefreshUser();

            return services;
        }
    }
}
