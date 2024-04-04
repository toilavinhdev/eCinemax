using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.RoomAggregate;

public class Room : TimeTrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string CinemaId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<List<Seat>> Seats { get; set; } = default!;
    
    public int SeatCount { get; set; }
}