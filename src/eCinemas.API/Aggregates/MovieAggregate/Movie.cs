using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.MovieAggregate;

public class Movie : TrackingDocument 
{
    public string Title { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public MovieStatus Status { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string>? Genres { get; set; }
    
    public List<string>? ImageUrl { get; set; }
    
    public int? Rate { get; set; }
}

public enum MovieStatus
{
    Trailer,
    Released
}