using eCinemax.Server.Aggregates.BookingAggregate;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Application.Responses;

public class GetBookingResponse
{
    public string Id { get; set; } = default!;

    public string MovieTitle { get; set; } = default!;

    public string CinemaName { get; set; } = default!;

    public string CinemaAddress { get; set; } = default!;

    public List<BookingSeat> Seats { get; set; } = default!;
    
    public int Total { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime ShowTimeStartAt { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime PaymentExpiredAt { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime CreatedAt { get; set; }
}

