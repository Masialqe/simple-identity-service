namespace IdentityApp.Users.RefreshUser
{
    public static class RefreshUserServicesConfiguration
    {
        public static IServiceCollection AddRefreshUser(this IServiceCollection services)
        {
            services.AddScoped<IRefreshUserHandler, RefreshUserHandler>();
            services.AddScoped<IRefreshUserValidator, RefreshUserValidator>();

            return services;
        }
    }
}
