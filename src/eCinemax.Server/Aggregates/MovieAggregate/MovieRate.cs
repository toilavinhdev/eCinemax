using System.ComponentModel.DataAnnotations;
using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.MovieAggregate;

public class MovieRate : ModifierTrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string MovieId { get; set; } = default!;
    
    [Range(0, 10)]
    public int Rate { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public string? Comment { get; set; }
}