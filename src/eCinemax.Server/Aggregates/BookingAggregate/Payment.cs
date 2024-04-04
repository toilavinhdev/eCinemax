using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.BookingAggregate;

public class Payment
{
    public PaymentDestination Destination { get; set; }

    public string Content { get; set; } = default!;

    public int Amount { get; set; } = default!;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? PaidAt { get; set; }
    
    public PaymentStatus Status { get; set; }
}

public enum PaymentDestination
{
    VnPay = 0,
    Momo
}

public enum PaymentStatus
{
    Processing = 0,
    Success,
    Cancelled,
    Failed, 
    Expired
}