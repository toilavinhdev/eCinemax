using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Aggregates.MovieAggregate;

public class Movie : TimeTrackingDocument 
{
    public string Title { get; set; } = default!;

    public string Plot { get; set; } = default!;

    public List<string> Directors { get; set; } = default!;

    public List<string> Casts { get; set; } = default!;

    public List<string> Languages { get; set; } = default!;
    
    public MovieStatus Status { get; set; }

    public List<string> Genres { get; set; } = default!;

    public int Age { get; set; }

    public string PosterUrl { get; set; } = default!;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? ReleasedAt { get; set; }
    
    public long DurationMinutes { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string>? UserMarks { get; set; }
}

public enum MovieStatus
{
    ComingSoon = 0,
    NowShowing,
    StopShowing
}