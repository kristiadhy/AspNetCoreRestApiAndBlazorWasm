using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.ServiceInstallers;

public class JwtInstaller : IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        //Here we create environment variable using terminal (CMD run as administrator).
        //Command : setx ASPNETCORE_ENVIRONMENT "SecretKey" /M
        //By using /M we specify that we want a system variable and not local.
        var secretKey = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
    }
}
