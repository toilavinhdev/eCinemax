using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class ListMovieQuery : IAPIRequest<ListMovieResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
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

        var fluent = _movieCollection.Find(filter);
        var totalRecord = await fluent.CountDocumentsAsync(cancellationToken);
        var documents = await fluent
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync(cancellationToken);

        var data = documents
            .Select(
                x => new MovieViewModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    Title = x.Title,
                    ImageUrl = x.ImageUrls?.FirstOrDefault()
                }).ToList();
        var pagination = new Pagination(request.PageIndex, request.PageSize, (int)totalRecord);
        
        return APIResponse<ListMovieResponse>.IsSuccess(new ListMovieResponse(pagination, data));
    }
}