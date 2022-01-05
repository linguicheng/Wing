using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Wing.Configuration.ServiceBuilder;

namespace Wing.Auth
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddJwt(this IWingServiceBuilder wingBuilder)
        {
            return AddConfig(wingBuilder, null);
        }

        public static IWingServiceBuilder AddJwt(this IWingServiceBuilder wingBuilder, Func<HttpContext, bool> validatePermission)
        {
            return AddConfig(wingBuilder, validatePermission);
        }

        private static IWingServiceBuilder AddConfig(IWingServiceBuilder wingBuilder, Func<HttpContext, bool> validatePermission)
        {
            var config = wingBuilder.GetConfig<JwtSetting>("Jwt");
            if (config == null)
            {
                throw new ArgumentNullException(nameof(JwtSetting));
            }

            wingBuilder.Services.AddAuthorization(options =>
            {
                if (validatePermission == null)
                {
                    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                    {
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireClaim(ClaimTypes.Name);
                    });
                    return;
                }

                options.AddPolicy(config.PolicyName, policy => policy.Requirements.Add(new JwtAuthRequirement() { ValidatePermission = validatePermission }));
            })
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = config.Iss,
                    ValidateAudience = true,
                    ValidAudience = config.Aud,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
            });
            if (validatePermission != null)
            {
                wingBuilder.Services.AddSingleton<IAuthorizationHandler, JwtAuthHandler>();
            }

            wingBuilder.Services.AddSingleton(typeof(IAuth), new JwtAuth(config));
            return wingBuilder;
        }
    }
}
