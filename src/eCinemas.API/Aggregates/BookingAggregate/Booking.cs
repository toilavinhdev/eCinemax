using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.BookingAggregate;

public class Booking : ModifierTrackingDocument
{
    public string ShowTimeId { get; set; } = default!;

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