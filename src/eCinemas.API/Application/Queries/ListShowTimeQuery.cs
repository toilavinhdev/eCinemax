using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class ListShowTimeQuery : IAPIRequest<List<CinemaShowTime>>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string MovieId { get; set; } = default!;

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

public class ListShowTimeQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListShowTimeQuery, List<CinemaShowTime>>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<List<CinemaShowTime>>> Handle(ListShowTimeQuery request, CancellationToken cancellationToken)
    {
        var filterBuilder = Builders<ShowTime>.Filter;
        var filter = filterBuilder.Empty;
        var parsedTime = DateTimeOffset.Parse(request.Date);
        filter &= filterBuilder.Eq(x => x.Movie, request.MovieId);
        filter &= filterBuilder.Where(x => x.StartAt >= parsedTime && x.StartAt < parsedTime.AddDays(1));

        var showTimes = await _showTimeCollection.Find(filter)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync(cancellationToken);
        
        var cinemaIds = showTimes.Select(x => x.Cinema).Distinct();
        var cinemas = await _cinemaCollection
            .Find(Builders<Cinema>.Filter.In(x => x.Id, cinemaIds))
            .ToListAsync(cancellationToken);

        if (cinemas.Count == 0) throw new ArgumentException("An error occurs when the data is incorrect.");

        var data = cinemas
            .Select(cinema => new CinemaShowTime
            {
                CinemaId = cinema.Id,
                CinemaName = cinema.Name,
                CinemaAddress = cinema.Address,
                ShowTimes = showTimes
                    .Where(showTime => showTime.Cinema == cinema.Id)
                    .Select(
                        x => new ShowTimeValue
                        {
                            ShowTimeId = x.Id, 
                            StartAt = x.StartAt, 
                            Available = x.Available
                        })
                    .ToList()
            }).ToList();

        return APIResponse<List<CinemaShowTime>>.IsSuccess(data);
    }
}