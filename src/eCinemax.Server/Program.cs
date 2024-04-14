using eCinemax.Server;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Infrastructure.Schedule;
using eCinemax.Server.Infrastructure.Services;
using eCinemax.Server.Shared.Extensions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using Todo.NET.Extensions;
using Todo.NET.Hangfire;
using Todo.NET.Security;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment<AppSettings>("AppSettings", out var appSettings);
builder.SetupSerilog();

var services = builder.Services;
services.AddCors();
services.AddSwaggerDocument();
services.AddHttpContextAccessor();
services.AddEndpointDefinitions(Metadata.Assembly);
services.AddJwtBearerAuth(appSettings.JwtConfig.TokenSingingKey);
services.AddAuthorization();
// services.AddHangfireMongo("Hangfire",
//     appSettings.MongoConfig.ConnectionString, 
//     appSettings.MongoConfig.DatabaseName);
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
app.UsePhysicalStaticFiles(
    appSettings.StaticFileConfig.Location, 
    appSettings.StaticFileConfig.External);
app.UseHttpsRedirection();
app.MapEndpointDefinitions();
// app.UseHangfireManagement(
//     appSettings.HangfireConfig.Title,
//     appSettings.HangfireConfig.UserName,
//     appSettings.HangfireConfig.Password);
// app.UseHangfireRecurringJobs();

app.Map("/", () => "eCinemax.Server");
app.MapGet("/ping", () => "Pong");
app.MapGet("/check-auth", () => "OK").RequireAuthorization();

await MongoInitialization.SeedAsync(app.Services);

app.Run();