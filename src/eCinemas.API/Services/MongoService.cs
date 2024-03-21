using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace eCinemas.API.Services;

public interface IMongoService : IBaseService
{
    IMongoCollection<T> Collection<T>() where T : Document;
}

public class MongoService : BaseService, IMongoService
{
    private readonly IMongoDatabase _database;
    
    public MongoService(AppSettings appSettings, ILogger<MongoService> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        var settings = MongoClientSettings.FromConnectionString(appSettings.MongoConfig.ConnectionString);
        settings.ClusterConfigurator = builder =>
        {
            builder.Subscribe<CommandStartedEvent>(e =>
            {
                logger.LogInformation($"Mongo driver execute command {e.CommandName}: {e.Command.ToJson()}");
            });
        };
        var client = new MongoClient(settings);
        _database = client.GetDatabase(appSettings.MongoConfig.DatabaseName);
    }

    public IMongoCollection<T> Collection<T>() where T : Document => _database.GetCollection<T>(typeof(T).Name);
}