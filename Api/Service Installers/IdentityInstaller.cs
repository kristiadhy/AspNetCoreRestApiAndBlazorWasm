﻿using Api.ServiceInstallers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Api.Service_Installers;

public class IdentityInstaller : IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        var builder = services.AddIdentity<UserModel, IdentityRole>(o =>
        {
            //Set rule for password
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;

            //Set rule for user
            o.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<AppDBContext>()
        .AddDefaultTokenProviders();
    }
}
