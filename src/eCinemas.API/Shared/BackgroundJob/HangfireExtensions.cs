using System.Reflection;
using eCinemas.API.Shared.ValueObjects;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using HangfireBasicAuthenticationFilter;
using MongoDB.Driver;

namespace eCinemas.API.Shared.BackgroundJob;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireBackgroundJob(this IServiceCollection services, MongoConfig mongoConfig)
    {
        services.AddHangfire(
            config =>
            {
                config
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMongoStorage(
                        new MongoClient(mongoConfig.ConnectionString),
                        mongoConfig.DatabaseName,
                        new MongoStorageOptions
                        {
                            MigrationOptions = new MongoMigrationOptions
                            {
                                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                                BackupStrategy = new CollectionMongoBackupStrategy()
                            },
                            Prefix = "Hangfire",
                            CheckConnection = true,
                            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                        });
            });
        services.AddHangfireServer();
        return services;
    }

    public static WebApplication UseHangfireBackgroundJob(this WebApplication app, HangfireConfig config)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DashboardTitle = config.Title,
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = config.Authentication.UserName,
                    Pass = config.Authentication.Password
                }
            },
            IgnoreAntiforgeryToken = true
        });
        return app;
    }

    public static WebApplication UseRecurringJobDefinitions(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var jobs = scope.ServiceProvider.GetServices<IHangfireCronJob>();
        
        foreach (var recurringJob in jobs)
        {
            var name = recurringJob.GetType().Name;
            var attribute = recurringJob.GetType().GetCustomAttribute<HangfireAttribute>();
            if (attribute is null)
                throw new ArgumentNullException($"{name} can be not null ${nameof(HangfireAttribute)}");
            var cron = attribute.Cron;
            
            RecurringJob.AddOrUpdate(name, () => recurringJob.Run(), cron);
        }

        return app;
    }
}