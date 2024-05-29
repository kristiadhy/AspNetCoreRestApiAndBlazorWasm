var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var host = builder.Host;

builder.Services.ConfigureServices(configuration, host);

//By default, ASP.NET Core adds a server header. Please refer to this link for more details : https://www.youtube.com/watch?v=ZqQOm0PK6ME&t=2343s
builder.WebHost.UseKestrel(sp => sp.AddServerHeader = false);

var app = builder.Build();
var env = app.Environment;

app.UseAppConfigurations(env);

app.Run();