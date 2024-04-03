using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.RoomAggregate;

public class Room : TimeTrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string CinemaId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<List<Seat>> Seats { get; set; } = default!;
    
    public int SeatCount { get; set; }
}