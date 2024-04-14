using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Aggregates.RoomAggregate;
using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Shared.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Todo.NET.Extensions;

namespace eCinemax.Server.Infrastructure.Persistence;

public static class MongoInitialization
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var mongoService = scope.ServiceProvider.GetRequiredService<IMongoService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IMongoService>>();
        logger.LogInformation("Start initializing mongo documents");
        await SeedAsync<User>(mongoService, "users.json");
        await SeedAsync<Cinema>(mongoService, "cinemas.json");
        await SeedAsync<Room>(mongoService, "rooms.json");
        await SeedAsync<Movie>(mongoService, "movie.json");
        logger.LogInformation("Finished initializing mongo documents");
    }
    
    private static async Task SeedAsync<TDocument>(IMongoService mongoService, string fileName) where TDocument : Document
    {
        var collection = mongoService.Collection<TDocument>();
        var any = await collection.Find(_ => true).AnyAsync();
        if (any) return;
        var path = Path.Combine("Infrastructure", "DocumentData", fileName);
        var json = await File.ReadAllTextAsync(path);
        if(string.IsNullOrEmpty(json)) return;
        var data = json.ToObject<List<TDocument>>();
        await collection.InsertManyAsync(data);
    }
}