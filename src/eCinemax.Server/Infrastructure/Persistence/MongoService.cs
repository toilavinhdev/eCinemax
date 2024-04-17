using eCinemax.Server.Infrastructure.Services;
using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace eCinemax.Server.Infrastructure.Persistence;

public interface IMongoService : IBaseService
{
    IMongoCollection<T> Collection<T>() where T : Document;

    Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions? options = null);
}

public class MongoService : BaseService, IMongoService
{
    private readonly IMongoDatabase _database;
    private readonly MongoClient _client;
    
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
        _client = new MongoClient(settings);
        _database = _client.GetDatabase(appSettings.MongoConfig.DatabaseName);
    }

    public IMongoCollection<T> Collection<T>() where T : Document => _database.GetCollection<T>(typeof(T).Name);

    public async Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions? options = null) 
        => await _client.StartSessionAsync(options);
}