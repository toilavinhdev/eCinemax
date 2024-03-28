using eCinemas.API.Aggregates.RoomAggregate;

namespace eCinemas.API.Aggregates.ShowtimeAggregate;

public class Reservation : Seat
{
    public SeatStatus Status { get; set; }
    
    public DateTimeOffset? ReservationAt { get; set; }

    public void MarkSoldOut()
    {
        Status = SeatStatus.SoldOut;
        ReservationAt = DateTimeOffset.Now;
    }
}