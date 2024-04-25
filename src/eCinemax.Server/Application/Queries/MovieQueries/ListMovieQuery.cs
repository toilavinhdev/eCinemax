using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Application.Responses;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.MovieQueries;

public class ListMovieQuery : IAPIRequest<ListMovieResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public MovieStatus Status { get; set; }

    public bool QueryMark { get; set; } = false;
}

public class ListMovieQueryValidator : AbstractValidator<ListMovieQuery>
{
    public ListMovieQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
    }
}

public class ListMovieQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListMovieQuery, ListMovieResponse>
{
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
        
    public async Task<APIResponse<ListMovieResponse>> Handle(ListMovieQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = mongoService.UserClaims().Id;
        
        var builder = Builders<Movie>.Filter;
        var filter = builder.Empty;
        
        // TODO: Cần optimize query cho UserMarks với dữ liệu lớn
        filter &= !request.QueryMark
            ? builder.Eq(x => x.Status, request.Status)
            : builder.Where(x => x.UserMarks!.Any(y => y == currentUserId));

        var fluent = _movieCollection.Find(filter);
        var totalRecord = await fluent.CountDocumentsAsync(cancellationToken);
        var documents = await fluent
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .Project(x => new MovieViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                PosterUrl = x.PosterUrl,
            })
            .ToListAsync(cancellationToken);

        return APIResponse<ListMovieResponse>.IsSuccess(
            new ListMovieResponse(
                documents, 
                request.PageIndex, 
                request.PageSize, 
                (int)totalRecord));
    }
}