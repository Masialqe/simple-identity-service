namespace IdentityApp.Users.LoginUser
{
    public static class LoginUserServicesConfiguration
    {
        public static IServiceCollection AddLoginUser(this IServiceCollection services)
        {
            services.AddScoped<ILoginUserHandler, LoginUserHandler>();
            services.AddScoped<ILoginUserValidator, LoginUserValidator>();

            return services;
        }
    }
}
