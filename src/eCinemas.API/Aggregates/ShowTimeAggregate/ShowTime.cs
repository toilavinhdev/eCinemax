using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Aggregates.ShowtimeAggregate;

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

    public List<SeatPrice> Ticket { get; set; } = default!;
    
    public int Available { get; set; }
    
    public List<List<Reservation>> Reservations { get; set; } = default!;
    
    public ShowTimeStatus Status { get; set; }
}