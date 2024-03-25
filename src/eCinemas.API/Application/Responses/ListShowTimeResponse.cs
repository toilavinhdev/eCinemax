namespace eCinemas.API.Application.Responses;

public class CinemaShowTime
{
    public string CinemaId { get; set; } = default!;

    public string CinemaName { get; set; } = default!;

    public string CinemaAddress { get; set; } = default!;

    public List<ShowTimeValue> ShowTimes { get; set; } = default!;
}

public class ShowTimeValue
{
    public string ShowTimeId { get; set; } = default!;
    
    public DateTimeOffset StartAt { get; set; }
    
    public int Available { get; set; }
}
