using eCinemas.API.Aggregates.RoomAggregate;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.ShowtimeAggregate;

public class Reservation : Seat
{
    public ReservationStatus Status { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? ReservationAt { get; set; }
    
    public string? ReservationBy { get; set; }
}

public enum ReservationStatus
{
    Idle,
    AwaitingPayment,
    SoldOut,
}