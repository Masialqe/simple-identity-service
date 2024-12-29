using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace IdentityApp.Endpoints
{
    /// <summary>
    /// Provides extension methods for registering and mapping endpoints in the application.
    /// </summary>
    public static class EndpointExtensions
    {
        /// <summary>
        /// Registers all endpoints in the current executing assembly that implement <see cref="IEndpoint"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register the endpoints with.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEndpoints(this IServiceCollection services)
        {
            services.AddEndpoints(Assembly.GetExecutingAssembly());
            return services;
        }

        /// <summary>
        /// Registers all endpoints in the specified assembly that implement <see cref="IEndpoint"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register the endpoints with.</param>
        /// <param name="assembly">The assembly to scan for endpoint implementations.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            ServiceDescriptor[] serviceDescriptors = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                               type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);

            return services;
        }

        /// <summary>
        /// Maps all registered endpoints that implement <see cref="IEndpoint"/> to the application's routing pipeline.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> to map the endpoints to.</param>
        /// <param name="routeGroupBuilder">
        /// An optional <see cref="RouteGroupBuilder"/> to group endpoints under a specific route. If null, endpoints are added to the root route.
        /// </param>
        /// <returns>The updated <see cref="WebApplication"/>.</returns>
        public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
        {
            IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(builder);
            }

            return app;
        }
    }

}
