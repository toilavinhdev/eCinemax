using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class ListShowTimeQuery : IAPIRequest<ListShowTimeResponse>, IPaginationRequest
{
    public string MovieId { get; set; } = default!;
    
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }

    public string Date { get; set; } = default!;
}

public class ListShowTimeQueryValidator : AbstractValidator<ListShowTimeQuery>
{
    public ListShowTimeQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.MovieId).NotEmpty();
    }
}

public class ListShowTimeQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListShowTimeQuery, ListShowTimeResponse>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse<ListShowTimeResponse>> Handle(ListShowTimeQuery request, CancellationToken cancellationToken)
    {
        var filterBuilder = Builders<ShowTime>.Filter;
        var filter = filterBuilder.Empty;

        var parsedTime = DateTimeOffset.Parse(request.Date);
        filter &= filterBuilder.Eq(x => x.Movie, request.MovieId);
        filter &= filterBuilder.Where(x => x.StartAt >= parsedTime && x.StartAt < parsedTime.AddDays(1));

        var fluent = _showTimeCollection.Find(filter);
        var totalRecord = await fluent.CountDocumentsAsync(cancellationToken);
        var data = await fluent
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .Project(x => 
                new ShowTimeListView
                {
                    Id = x.Id,
                    MovieId = x.Movie,
                    StartAt = x.StartAt,
                })
            .ToListAsync(cancellationToken);

        return APIResponse<ListShowTimeResponse>
            .IsSuccess(
                new ListShowTimeResponse(
                    data, 
                    request.PageIndex,
                    request.PageSize, 
                    (int)totalRecord));
    }
}