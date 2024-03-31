using eCinemas.API.Aggregates.RoomAggregate;

namespace eCinemas.API.Aggregates.ShowtimeAggregate;

public class SeatPrice
{
    public SeatType Type { get; set; }
    
    public int Price { get; set; }
}