using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Shared.ValueObjects;

namespace eCinemax.Server.Application.Responses;

public class ListMovieRateResponse(List<MovieRate> records, int pageIndex, 
    int pageSize, 
    int totalRecord) : PaginationResponse<MovieRate>(records, pageIndex, pageSize, totalRecord);

public class MovieRateViewModel
{
    public string User { get; set; } = default!;
    
    public string MovieId { get; set; } = default!;
    
    public int Rate { get; set; }
    
    public string? Comment { get; set; }
}