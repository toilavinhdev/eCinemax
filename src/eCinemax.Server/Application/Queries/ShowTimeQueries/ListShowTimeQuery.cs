﻿using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Persistence;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.ShowTimeQueries;

public class ListShowTimeQuery : IAPIRequest<List<CinemaShowTime>>
{
    public string MovieId { get; set; } = default!;
    
    public DateTime ShowDate { get; set; } = default!;
}

public class ListShowTimeQueryValidator : AbstractValidator<ListShowTimeQuery>
{
    public ListShowTimeQueryValidator()
    {
        RuleFor(x => x.MovieId).NotEmpty().WithMessage("Không để trống phim");
        RuleFor(x => x.ShowDate).NotEmpty().WithMessage("Không để trống ngày chiếu");
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
        filter &= filterBuilder.Eq(x => x.MovieId, request.MovieId);
        filter &= filterBuilder.Where(x =>
            x.StartAt >= request.ShowDate.ToLocalTime().Date &&
            x.StartAt < request.ShowDate.ToLocalTime().Date.AddDays(1));
        filter &= filterBuilder.Eq(x => x.Status, ShowTimeStatus.Upcoming);
        
        var showTimes = await _showTimeCollection
            .Find(filter)
            .ToListAsync(cancellationToken);
        
        if (showTimes.Count == 0) return APIResponse<List<CinemaShowTime>>.IsSuccess([]);
        
        var cinemaIds = showTimes.Select(x => x.CinemaId).Distinct();
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
                    .Where(showTime => showTime.CinemaId == cinema.Id)
                    .Select(
                        x => new ShowTimeValue
                        {
                            ShowTimeId = x.Id, 
                            StartAt = x.StartAt, 
                            Available = x.Available
                        })
                    .OrderBy(x => x.StartAt)
                    .ToList()
            }).ToList();

        return APIResponse<List<CinemaShowTime>>.IsSuccess(data);
    }
}