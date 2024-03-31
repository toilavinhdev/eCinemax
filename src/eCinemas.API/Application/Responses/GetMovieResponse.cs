using eCinemas.API.Aggregates.MovieAggregate;

namespace eCinemas.API.Application.Responses;

public class GetMovieResponse
{
    public string Id { get; set; } = default!;
    
    public string Title { get; set; } = default!;

    public string Plot { get; set; } = default!;

    public List<string> Directors { get; set; } = default!;

    public List<string> Casts { get; set; } = default!;

    public List<string> Languages { get; set; } = default!;
    
    public MovieStatus Status { get; set; }

    public List<string> Genres { get; set; } = default!;

    public int Age { get; set; }

    public string PosterUrl { get; set; } = default!;
    
    public DateTime? ReleasedAt { get; set; }
    
    public long DurationMinutes { get; set; }
}