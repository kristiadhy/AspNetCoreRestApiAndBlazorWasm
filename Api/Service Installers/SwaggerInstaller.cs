
using Microsoft.OpenApi.Models;

namespace Api.ServiceInstallers;

public class SwaggerInstaller : IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CleanArchitecture.Api",
                Version = "v1"
            });

            //Here we set the swagger to use bearer token authorization
            var jwtSecurityScheme = new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please provide a valid token",
                Name = "Authorization", //IMPORTANT! : Please don't change with anything other than Authorization
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            };
            options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }

                    },
                    new List<string>()
                }
            });
        });
    }
}
