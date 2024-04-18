using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.MovieAggregate;

public class MovieReview : ModifierTrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string MovieId { get; set; } = default!;
    
    public int Rate { get; set; }

    public string? Review { get; set; }
}