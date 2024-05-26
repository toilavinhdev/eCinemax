using eCinemax.Server.Persistence;
using eCinemax.Server.Schedule;
using eCinemax.Server.Services;
using Todo.NET.Extensions;
using Todo.NET.Hangfire;
using Todo.NET.Security;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment<AppSettings>(nameof(AppSettings), out var appSettings);
builder.SetupSerilog();

var services = builder.Services;
services.AddPolicyCors(Metadata.Name);
services.AddSwaggerDocument(Metadata.Name, "v1");
services.AddHttpContextAccessor();
services.AddSignalRManager();
services.AddEndpointDefinitions(Metadata.Assembly);
services.AddJwtBearerAuth(appSettings.JwtConfig.TokenSingingKey).AddAuthorization();
services.AddHangfireMongo(o =>
{
    o.Prefix = "Hangfire";
    o.ConnectionString = appSettings.MongoConfig.ConnectionString;
    o.DatabaseName = appSettings.MongoConfig.DatabaseName;
});
services.AddValidatorsFromAssembly(Metadata.Assembly);
services.AddMediatR(config =>
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
services.AddSingleton<ReservationGroupService>();
services.AddSingleton<ReservationHub>();

var app = builder.Build();
app.UseDefaultExceptionHandler();
app.UsePolicyCors(Metadata.Name);
app.UseSwaggerDocument(Metadata.Name);
app.UsePhysicalStaticFiles(
    appSettings.StaticFileConfig.Location,
    appSettings.StaticFileConfig.External);
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
app.MapEndpointDefinitions();
app.MapHub<NotificationHub>("/notification").RequireAuthorization();
app.MapHub<ReservationHub>("/reservation").RequireAuthorization();

await MongoInitialization.SeedAsync(app.Services);

if (builder.Environment.IsDevelopment())
{
    app.Urls.Add("http://localhost:5005");
    app.Urls.Add($"http://{IPExtensions.GetLocalIPAddress()}:5015");
}

app.Map("/", () => Metadata.Name);
app.Run();