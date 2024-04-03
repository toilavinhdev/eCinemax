using eCinemas.API;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Infrastructure.Schedule;
using eCinemas.API.Infrastructure.Services;
using eCinemas.API.Shared.BackgroundJob;
using eCinemas.API.Shared.Extensions;
using eCinemas.API.Shared.Mediator;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment("AppSettings", out var appSettings);
builder.SetupSerilog();

var services = builder.Services;
services.AddCors();
services.AddSwaggerDocument();
services.AddHttpContextAccessor();
services.AddEndpointDefinitions(Metadata.Assembly);
services.AddJwtBearerAuth(appSettings.JwtConfig);
services.AddAuthorization();
services.AddHangfireBackgroundJob(appSettings.MongoConfig);
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
services.AddScoped<IMongoService, MongoService>();
services.AddScoped<IHangfireCronJob, ShowTimeStatusTrackingService>();
services.AddScoped<IHangfireCronJob, BookingStatusTrackingService>();

var app = builder.Build();
app.UseDefaultExceptionHandler();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseSwaggerDocument();
app.UseAuthentication();
app.UseAuthorization();
app.UsePhysicalStaticFile(appSettings.StaticFileConfig);
app.UseHttpsRedirection();
app.MapEndpointDefinitions();
app.UseHangfireBackgroundJob(appSettings.HangfireConfig);
app.UseRecurringJobDefinitions();
app.MapGet("/ping", () => "Pong");
app.MapGet("/check-auth", () => "OK").RequireAuthorization();

app.Run();