using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Application.Responses;

public class ListMovieResponse(List<MovieViewModel> records, int pageIndex, 
    int pageSize, int totalRecord) : PaginationResponse<MovieViewModel>(records, pageIndex, pageSize, totalRecord);

public class MovieViewModel
{
    public string Id { get; set; } = default!;
    
    public string Title { get; set; } = default!;
    
    public MovieStatus Status { get; set; }
    
    public string PosterUrl { get; set; } = default!;
}