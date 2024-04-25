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
app.MapHub<NotificationSignalRHub>("/hub/notification").RequireAuthorization();
app.MapHub<ReservationHub>("/hub/reservation").RequireAuthorization();

await MongoInitialization.SeedAsync(app.Services);

if (builder.Environment.IsDevelopment())
{
    var localIpAddress = IPExtensions.GetLocalIPAddress();
    app.Urls.Add("http://localhost:5005");
    app.Urls.Add($"http://{localIpAddress}:5015");
}

app.MapGet("/", () => Metadata.Name);
app.MapGet("/health", () => "OK");
app.MapGet("/auth-check", () => "DONE").RequireAuthorization();
app.MapGet("/hub-connections", (ConnectionManager connectionManager) => new
{
    TotalUser = connectionManager.Connections.Keys.Count,
    TotalConnection = connectionManager.Connections.Values.SelectMany(x => x).ToList().Count,
    Data = connectionManager.Connections
});
app.Run();