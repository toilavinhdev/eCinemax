using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.PaymentAggregate;

public class Payment : TrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string ShowTime { get; set; } = default!;

    public List<Seat> Seats { get; set; } = default!;
    
    public int Total { get; set; }
}