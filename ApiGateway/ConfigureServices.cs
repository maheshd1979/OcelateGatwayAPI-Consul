using ApiGateway.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add endpoint definitions based on the specified marker interface (IEndPoint)


            services.AddScoped<ITokenManager, TokenManager>();

            // Configure and register JWT authentication
            services.AddJwtAuthenticationConfiguration(configuration);
            return services;
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
            var secret = Encoding.ASCII.GetBytes(tokenModel.SecretKey);

            // Configure the authentication services with JWT
            services.AddAuthentication(x =>
            {
                // Set the default authentication scheme for both authentication and challenge
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                // Configure JWT Bearer authentication options
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;

                // Configure token validation parameters
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = tokenModel.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenModel.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }


    }
}
