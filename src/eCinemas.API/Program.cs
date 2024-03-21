using eCinemas.API;
using eCinemas.API.Extensions;
using eCinemas.API.Mediator;
using eCinemas.API.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment("AppSettings", out var appSettings);

var services = builder.Services;
services.AddCors();
services.AddSwaggerDocument();
services.AddEndpointDefinitions(Metadata.Assembly);
services.AddAuthorization();
services.AddValidatorsFromAssembly(Metadata.Assembly);
services.AddMediatR(
    config =>
    {
        config.RegisterServicesFromAssembly(Metadata.Assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
services.AddAutoMapper(Metadata.Assembly);
services.AddTransient<IBaseService, BaseService>();
services.AddTransient<IStorageService, StorageService>();

var app = builder.Build();
app.UseDefaultExceptionHandler();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.UsePhysicalStaticFile(appSettings.StaticFileConfig);
app.UseHttpsRedirection();
app.MapEndpointDefinitions();
app.MapGet("Ping", () => "Pong");

app.Run();