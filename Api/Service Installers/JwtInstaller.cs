using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.ServiceInstallers;

public class JwtInstaller : IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        //Here we create environment variable using terminal (CMD run as administrator).
        //Command : setx SECRET "MyUniqueSecretKey" /M
        //By using /M we specify that we want a system variable and not local.
        //IMPORTANT : You need to restart your visual studio after setting environment variable to make it works! It's weird

        //IMPORTANT : There is a new rule in the latest Jwt Nuget Package. Read the explanation below:
        //This is a fix to ensure when creating a JWS(signed JsonWebToken), the key size is of sufficient size.
        //The key size must be > the size Hash algorithm returns.
        //HS256 requires 256 bits, HS384 requires 384 bits, HS512 requires 512 bits.
        //Allowance is made for authenticated encryption which uses a smaller key for the AuthenticationTag.
        //If the SymmetricKey is smaller than the required size IdentityModel will throw an ArgumentOutOfRangeException which is the same exception is the SymmetricKey is smaller than the minimum key size.
        //Reference : https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2072

        //We don't want to get the JwtSettings from appsettings.json file because it's error-prone. We are gonna use the setting quite often, so we prefer use the strong type by create new data model called JwtConfiguration
        //var jwtSettings = configuration.GetSection("JwtSettings");
        var jwtConfiguration = new JwtConfiguration();
        //We bind the configuration from the appsettings.json to the data model we created.
        configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

        var secretKey = Environment.GetEnvironmentVariable("SECRET");
        if (secretKey is null)
            return;

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
                ValidIssuer = jwtConfiguration.ValidIssuer,
                ValidAudience = jwtConfiguration.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }
}
