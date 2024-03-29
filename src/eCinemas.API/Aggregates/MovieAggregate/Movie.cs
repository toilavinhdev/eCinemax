using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Aggregates.MovieAggregate;

public class Movie : TimeTrackingDocument 
{
    public string Title { get; set; } = default!;

    public string Plot { get; set; } = default!;

    public List<string> Directors { get; set; } = default!;

    public List<string> Casts { get; set; } = default!;

    public List<string> Languages { get; set; } = default!;
    
    public MovieStatus Status { get; set; }

    public List<string> Genres { get; set; } = default!;

    public string PosterUrl { get; set; } = default!;
    
    public DateTimeOffset? ReleasedAt { get; set; }
    
    public long DurationMinutes { get; set; }
}