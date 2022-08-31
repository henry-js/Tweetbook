using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tweetbook.Options;
using Tweetbook.Services;

namespace Tweetbook.Installers;

public class ApiInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
        services.AddSingleton(jwtSettings);
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddControllers(); // TODO : switch back to using AddControllersWithViews if bugs occur

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        });

        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweetbook API", Version = "v1" });

            // var security = new Dictionary<string, IEnumerable<string>>
            // {
            //     {"Bearer ", Array.Empty<string>()}
            // };
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization"
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "JWT"}
                    },
                    new string[0]
                }
            });
        });
    }
}
