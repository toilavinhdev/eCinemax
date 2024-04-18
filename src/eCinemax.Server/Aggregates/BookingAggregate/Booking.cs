using eCinemax.Server.Aggregates.RoomAggregate;
using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.BookingAggregate;

public class Booking : ModifierTrackingDocument, IAggregateRoot
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string ShowTimeId { get; set; } = default!;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string MovieId { get; set; } = default!;

    public List<BookingSeat> Seats { get; set; } = default!;
    
    public int Total { get; set; }
    
    public BookingStatus Status { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime PaymentExpiredAt { get; set; }

    public Payment? Payment { get; set; }
}

public class BookingSeat
{
    public SeatType Type { get; set; }

    public List<string> SeatNames { get; set; } = default!;

    public int Quantity => SeatNames.Count;
}

public enum BookingStatus
{
    WaitForPay = 0,
    Success,
    Cancelled,
    Failed, 
    Expired
}