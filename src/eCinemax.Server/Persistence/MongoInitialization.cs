using eCinemax.Server.Aggregates.UserAggregate;
using MongoDB.Driver;
using Todo.NET.Extensions;

namespace eCinemax.Server.Persistence;

public static class MongoInitialization
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var mongoService = scope.ServiceProvider.GetRequiredService<IMongoService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IMongoService>>();
        logger.LogInformation("Start initializing mongo documents");
        logger.LogInformation("Finished initializing mongo documents");
    }
    
    private static async Task SeedAsync<TDocument>(IMongoService mongoService, string fileName) where TDocument : Document
    {
        var collection = mongoService.Collection<TDocument>();
        var any = await collection.Find(_ => true).AnyAsync();
        if (any) return;
        var path = Path.Combine("Persistence", "MigrateData", fileName);
        var json = await File.ReadAllTextAsync(path);
        if(string.IsNullOrEmpty(json)) return;
        var data = json.ToObject<List<TDocument>>();
        await collection.InsertManyAsync(data);
    }
}