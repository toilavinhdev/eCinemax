using eCinemas.API.Aggregates.ShowtimeAggregate;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Application.Responses;

public class GetShowTimeResponse
{
    public string Id { get; set; } = default!;
    
    public string MovieId { get; set; } = default!;

    public string CinemaName { get; set; } = default!;

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime StartAt { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime FinishAt { get; set; }

    public List<SeatPrice> Ticket { get; set; } = default!;
    
    public int Available { get; set; }
    
    public List<List<Reservation>> Reservations { get; set; } = default!;
}