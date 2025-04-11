using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthenticationJWT
{
    public static class JwtExtensions
    {
        // Define a constant security key used to sign and validate JWT tokens.
       
        public const string SecurityKey = "myecommercen#services1234567890enpoint#key";

        // Extension method to add JWT authentication to the service collection
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            // Configure the authentication services to use JWT Bearer tokens
            services.AddAuthentication(opt =>
            {
                // Set the default authentication scheme to use JWT Bearer tokens
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // Set the default challenge scheme to JWT Bearer for responding to authentication challenges
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Define the parameters for validating the incoming JWT tokens
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the issuer of the token (i.e., who created it)
                    ValidateIssuer = true,
                    ValidIssuer = "SpeedyGo-issuer", // The valid issuer of the token

                    // Validate the audience (i.e., who the token is intended for)
                    ValidateAudience = true,
                    ValidAudience = "SpeedyGo-audience", // The valid audience of the token

                    // Validate the signing key used to sign the token
                    ValidateIssuerSigningKey = true,
                    // Use the symmetric security key (secret) for validating the token signature
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
                };
            });
        }
    }
}
