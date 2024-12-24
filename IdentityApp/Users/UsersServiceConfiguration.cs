using IdentityApp.Users.CreateRole;
using IdentityApp.Users.CreateUser;
using IdentityApp.Users.LoginUser;
using IdentityApp.Users.RefreshUser;

namespace IdentityApp.Users
{
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
