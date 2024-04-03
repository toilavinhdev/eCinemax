using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries.MovieQueries;

public class ListMovieQuery : IAPIRequest<ListMovieResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public MovieStatus Status { get; set; }
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
        var builder = Builders<Movie>.Filter;
        var filter = builder.Empty;

        filter &= builder.Eq(x => x.Status, request.Status);

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