using eCinemas.API.Aggregates.MovieAggregate;

namespace eCinemas.API.Application.Responses;

public class GetMovieResponse
{
    public string Id { get; set; } = default!;
    
    public string Title { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public MovieStatus Status { get; set; }
    
    public List<string>? Genres { get; set; }
    
    public List<string>? ImageUrls { get; set; }
    
    public long DurationMinutes { get; set; }
}