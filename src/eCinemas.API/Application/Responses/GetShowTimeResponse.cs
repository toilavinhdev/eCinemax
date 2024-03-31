using eCinemas.API.Aggregates.ShowtimeAggregate;

namespace eCinemas.API.Application.Responses;

public class GetShowTimeResponse
{
    public string Id { get; set; } = default!;
    
    public string MovieId { get; set; } = default!;

    public string CinemaName { get; set; } = default!;

    public DateTimeOffset StartAt { get; set; }

    public List<SeatPrice> Ticket { get; set; } = default!;
    
    public List<List<Reservation>> Reservations { get; set; } = default!;
}