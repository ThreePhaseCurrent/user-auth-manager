using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UserAuthManager.API.Mapping;
using UserAuthManager.API.Models;
using UserAuthManager.API.Services;
using UserAuthManager.API.Services.Interfaces;

namespace UserAuthManager.API.Extentions
{
    /// <summary>
    /// Extensions for service collection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register services
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(ITokenService), typeof(TokenService));
            services.AddTransient(typeof(IUserService), typeof(UserService));
        }

        /// <summary>
        /// Configure auth with jwt
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthJwt(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<AuthOptions>(configuration);

            var authOptions = configuration.Get<AuthOptions>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        
                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        
                        ValidateLifetime = true,
                        
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });
            services.AddAuthorization();
        }

        public static void AutoMapperConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            var mapperConfig = new MapperConfiguration(expression =>
            {
                expression.AddProfile(new AutoMapping());
            });
            var mapper       = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}