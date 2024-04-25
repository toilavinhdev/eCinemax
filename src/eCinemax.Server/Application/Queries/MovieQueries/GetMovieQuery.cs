using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Application.Responses;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.MovieQueries;

public class GetMovieQuery : IAPIRequest<GetMovieResponse>
{
    public string Id { get; set; } = default!;
}

public class GetMovieQueryValidator : AbstractValidator<GetMovieQuery>
{
    public GetMovieQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Vui lòng chọn phim");
    }
}

public class GetMovieQueryHandler(IMongoService mongoService, IMapper mapper, IMediator mediator) : IAPIRequestHandler<GetMovieQuery, GetMovieResponse>
{
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    private readonly IMongoCollection<MovieReview> _reviewCollection = mongoService.Collection<MovieReview>();
    
    public async Task<APIResponse<GetMovieResponse>> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Movie>.ThrowIfNotFound(movie, "Không tìm thấy phim");
        
        var reviews = await mediator
            .Send(new ListReviewQuery
            {
                MovieId = movie.Id,
                PageIndex = 1,
                PageSize = 5
            }, cancellationToken);
        
        var data = mapper.Map<GetMovieResponse>(movie);
        data.Marked = movie.UserMarks.Any(x => x == mongoService.UserClaims().Id);
        data.Reviews = reviews.Data.Records;

        var rate = await _reviewCollection
            .Aggregate()
            .Match(x => x.MovieId == movie.Id)
            .Group(x => x.MovieId, group => new
            {
                AverageRate = group.Average(r => r.Rate),
                Total = group.Count()
            })
            .FirstOrDefaultAsync(cancellationToken);

        data.AverageRate = rate?.AverageRate ?? 0;
        data.TotalReview = rate?.Total ?? 0;
        
        return APIResponse<GetMovieResponse>.IsSuccess(data);
    }
}