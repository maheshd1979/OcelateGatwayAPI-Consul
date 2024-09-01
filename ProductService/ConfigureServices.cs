using ProductService.EndPoints;
using ProductService.Util;

namespace ProductService
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add endpoint definitions based on the specified marker interface (IEndPoint)
            services.AddEndpointDefinitions(typeof(IEndPoint));

            services.AddScoped<ITokenManager, TokenManager>();

            // Configure and register JWT authentication
            services.AddJwtAuthenticationConfiguration(configuration);
            return services;
        }

        public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMarkers)
        {
            // Create a list to store the discovered endpoint definitions
            var endpointDefinitions = new List<IEndPoint>();

            // Iterate through each scan marker
            foreach (var marker in scanMarkers)
            {
                // Find types in the assembly of the marker that implement IEndPoint and are not abstract or interfaces
                endpointDefinitions.AddRange(
                    marker.Assembly.ExportedTypes
                        .Where(x => typeof(IEndPoint).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .Select(Activator.CreateInstance).Cast<IEndPoint>()
                );
            }
            // Add the discovered endpoint definitions as a singleton service
            services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndPoint>);
        }

        /// <summary>
        /// Maps endpoint definitions to the specified WebApplication.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> to which endpoint definitions will be mapped.</param>

        public static void AddJwtAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure the TokenModel by binding the configuration section "TokenManagement"
            services.Configure<TokenModel>(configuration.GetSection("TokenManagement"));

            // Retrieve the TokenModel from the configuration
            var tokenModel = configuration.GetSection("TokenManagement").Get<TokenModel>();

            // Convert the secret key from a string to bytes

        }
        public static void UseEndpointDefinitions(this WebApplication app)
        {
            // Retrieve the registered endpoint definitions from the service collection
            var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndPoint>>();

            // Map each endpoint definition to the WebApplication
            foreach (var endpointDefinition in definitions)
            {
                endpointDefinition.MapEndpoint(app);
            }
        }

    }
}
