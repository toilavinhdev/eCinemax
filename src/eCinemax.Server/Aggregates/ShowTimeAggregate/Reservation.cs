using eCinemax.Server.Aggregates.RoomAggregate;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.ShowtimeAggregate;

public class Reservation : Seat
{
    public ReservationStatus Status { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? ReservationAt { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ReservationBy { get; set; }
}

public enum ReservationStatus
{
    Idle,
    AwaitingPayment,
    SoldOut,
}