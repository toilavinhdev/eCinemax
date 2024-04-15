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

var urls = new List<string>();
if (builder.Environment.IsDevelopment())
{
    var localIpAddress = IPExtensions.GetLocalIPAddress();
    urls.Add("http://localhost:5005");
    urls.Add($"http://{localIpAddress}:5015");
}

var services = builder.Services;
services.AddPolicyCors("eCinemax");
services.AddSwaggerDocument("eCinemax.Server", "v1");
services.AddHttpContextAccessor();
services.AddEndpointDefinitions(Metadata.Assembly);
services.AddJwtBearerAuth(appSettings.JwtConfig.TokenSingingKey).AddAuthorization();
services.AddHangfireMongo(o =>
{
    o.Prefix = "Hangfire";
    o.ConnectionString = appSettings.MongoConfig.ConnectionString;
    o.DatabaseName = appSettings.MongoConfig.DatabaseName;
});
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
app.UsePolicyCors("eCinemax");
app.UseSwaggerDocument("eCinemax API");
app.UsePhysicalStaticFiles(appSettings.StaticFileConfig.Location, appSettings.StaticFileConfig.External);
app.UseAuthentication().UseAuthorization();
app.UseHttpsRedirection();
app.UseHangfireManagement(c =>
{
    c.Title = appSettings.HangfireConfig.Title;
    c.Path = appSettings.HangfireConfig.Path;
    c.UserName = appSettings.HangfireConfig.UserName;
    c.Password = appSettings.HangfireConfig.Password;
});
app.UseHangfireRecurringJobs();

app.Map("/", () => "eCinemax.Server");
app.MapGet("/ping", () => "Pong");
app.MapGet("/check-auth", () => "OK").RequireAuthorization();
app.MapEndpointDefinitions();

await MongoInitialization.SeedAsync(app.Services);

urls.ForEach(url => app.Urls.Add(url));

app.Run();