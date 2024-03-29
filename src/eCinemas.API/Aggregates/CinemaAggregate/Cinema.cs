using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.CinemaAggregate;

public class Cinema : Document
{
    public string Name { get; set; } = default!;

    public string Address { get; set; } = default!;

    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> RoomIds { get; set; } = default!;
}