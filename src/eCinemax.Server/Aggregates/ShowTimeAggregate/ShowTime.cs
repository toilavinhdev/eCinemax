using eCinemax.Server.Aggregates.RoomAggregate;
using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.ShowtimeAggregate;

public class ShowTime : TimeTrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string MovieId { get; set; } = default!;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string CinemaId { get; set; } = default!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string RoomId { get; set; } = default!;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime StartAt { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime FinishAt { get; set; }

    public List<SeatPrice> Ticket { get; set; } = default!;
    
    public List<List<Reservation>> Reservations { get; set; } = default!;
    
    public ShowTimeStatus Status { get; set; }
    
    public int Available => Reservations
        .Aggregate(0, 
            (acc, cur) => acc + cur.Count(
                x => x.Type != SeatType.Blank && 
                     x.Status == ReservationStatus.Idle));
}

public class SeatPrice
{
    public SeatType Type { get; set; }
    
    public int Price { get; set; }
}

public enum ShowTimeStatus
{
    Upcoming = 0,
    NowShowing,
    Cancelled,
    Finished
}