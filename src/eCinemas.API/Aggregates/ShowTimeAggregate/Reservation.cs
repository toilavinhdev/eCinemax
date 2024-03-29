using eCinemas.API.Aggregates.RoomAggregate;

namespace eCinemas.API.Aggregates.ShowtimeAggregate;

public class Reservation : Seat
{
    public ReservationStatus Status { get; set; }
    
    public DateTimeOffset? ReservationAt { get; set; }
}