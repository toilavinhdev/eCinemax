using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.ShowtimeAggregate;

public class ShowTime : TrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Movie { get; set; } = default!;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string Cinema { get; set; } = default!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string Room { get; set; } = default!;
    
    public DateTimeOffset StartAt { get; set; }

    public List<SeatPrice> Ticket { get; set; } = default!;
    
    public int Available { get; set; }
    
    public List<List<Reservation>> Reservations { get; set; } = default!;

    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Bookings { get; set; } = default!;
}

public class SeatPrice
{
    public SeatType Type { get; set; }
    
    public int Price { get; set; }
}

public class Reservation : Seat
{
    public SeatStatus Status { get; set; }
    
    public DateTimeOffset? ReservationAt { get; set; }
}