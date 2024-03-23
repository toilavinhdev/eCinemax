using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Application.Responses;

public class ListMovieResponse(
    Pagination pagination, 
    List<MovieViewModel> records): PaginationResponse<MovieViewModel>(pagination, records);

public class MovieViewModel
{
    public string Id { get; set; } = default!;
    
    public string Title { get; set; } = default!;

    public MovieStatus Status { get; set; }
    
    public string? ImageUrl { get; set; }
}