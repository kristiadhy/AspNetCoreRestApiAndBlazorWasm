var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var host = builder.Host;

builder.Services.ConfigureServices(configuration, host);

var app = builder.Build();
var env = app.Environment;

app.UseAppConfigurations(env);

app.Run();