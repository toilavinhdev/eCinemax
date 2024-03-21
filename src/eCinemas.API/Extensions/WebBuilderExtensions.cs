using eCinemas.API.ValueObjects;

namespace eCinemas.API.Extensions;

public static class WebBuilderExtensions
{
    public static void SetupEnvironment(this WebApplicationBuilder builder, string? external, out AppSettings appSettings)
    {
        appSettings = new AppSettings();
        var environment = builder.Environment;
        var json = $"appsettings.{environment.EnvironmentName}.json";
        var path = string.IsNullOrEmpty(external) ? json : Path.Combine(external, json);
        
        new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile(path)
            .AddEnvironmentVariables()
            .Build()
            .Bind(appSettings);

        builder.Services.AddSingleton(appSettings);
    }
}