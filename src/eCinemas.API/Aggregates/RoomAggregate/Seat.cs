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
    Blank = 0,
    Normal,
    VIP,
    Couple
}