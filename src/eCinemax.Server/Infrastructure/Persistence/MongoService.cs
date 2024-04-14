using eCinemax.Server.Infrastructure.Services;
using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemax.Server.Infrastructure.Persistence;

public interface IMongoService : IBaseService
{
    IMongoCollection<T> Collection<T>() where T : Document;
}

public class MongoService : BaseService, IMongoService
{
    private readonly IMongoDatabase _database;
    
    public MongoService(AppSettings appSettings, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        var settings = MongoClientSettings.FromConnectionString(appSettings.MongoConfig.ConnectionString);
        var client = new MongoClient(settings);
        _database = client.GetDatabase(appSettings.MongoConfig.DatabaseName);
    }

    public IMongoCollection<T> Collection<T>() where T : Document => _database.GetCollection<T>(typeof(T).Name);
}