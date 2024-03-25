using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Application.Responses;

public class ListMovieResponse(List<MovieViewList> records, int pageIndex, 
    int pageSize, int totalRecord) : PaginationResponse<MovieViewList>(records, pageIndex, pageSize, totalRecord);

public class MovieViewList
{
    public string Id { get; set; } = default!;
    
    public string Title { get; set; } = default!;

    public MovieStatus Status { get; set; }

    public string PosterUrl { get; set; } = default!;
    
    public long DurationMinutes { get; set; }
}