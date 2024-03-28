namespace eCinemas.API.Aggregates.RoomAggregate;

public class Seat
{
    public string Row { get; set; } = default!;

    public int Column { get; set; }

    public string Name => $"{Row}{Column}";
    
    public SeatType Type { get; set; }
}

public enum SeatType
{
    Empty = 0,
    Normal,
    VIP,
    Couple
}

public enum SeatStatus
{
    Empty = 0,
    SoldOut
}