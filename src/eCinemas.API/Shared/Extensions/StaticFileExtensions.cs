using eCinemas.API.Shared.ValueObjects;
using Microsoft.Extensions.FileProviders;

namespace eCinemas.API.Shared.Extensions;

public static class StaticFileExtensions
{
    public static IApplicationBuilder UsePhysicalStaticFile(this IApplicationBuilder app, StaticFileConfig config)
    {
        if (!Directory.Exists(config.Location)) Directory.CreateDirectory(config.Location);
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(config.Location),
            RequestPath = new PathString(config.External)
        });
        return app;
    }
}