using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.RoomAggregate;

public class Room : TrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Cinema { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<List<Seat>> Seats { get; set; } = default!;
}

public class Seat
{
    public string Row { get; set; } = default!;

    public int Column { get; set; }

    public string Name => $"{Row}{Column}";
    
    public SeatType Type { get; set; }
}

public enum SeatType
{
    Empty = 0,
    Normal,
    VIP,
    Couple
}

public enum SeatStatus
{
    Empty = 0,
    SoldOut
}