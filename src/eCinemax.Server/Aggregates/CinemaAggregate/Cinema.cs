using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.CinemaAggregate;

public class Cinema : Document, IAggregateRoot
{
    public string Name { get; set; } = default!;

    public string Address { get; set; } = default!;
    
    public CinemaLocation? Location { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> RoomIds { get; set; } = default!;
}