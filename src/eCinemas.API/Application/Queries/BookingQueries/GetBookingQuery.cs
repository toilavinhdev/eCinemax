using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries.BookingQueries;

public class GetBookingQuery : IAPIRequest<GetBookingResponse>
{
    public string Id { get; set; } = default!;
}

public class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
{
    public GetBookingQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetBookingQueryHandler(IMongoService mongoService) : IAPIRequestHandler<GetBookingQuery, GetBookingResponse>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    
    public async Task<APIResponse<GetBookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var document = await _bookingCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Booking>.ThrowIfNotFound(document, "Đơn hàng không tồn tại");

        var showTime = await _showTimeCollection
            .Find(x => x.Id == document.ShowTimeId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<ShowTime>.ThrowIfNotFound(showTime, "Không tìm thấy lịch chiếu");

        var cinema = await _cinemaCollection
            .Find(x => x.Id == showTime.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Cinema>.ThrowIfNotFound(cinema, "Không tìm thấy rạp chiếu");
        
        var movie = await _movieCollection
            .Find(x => x.Id == showTime.MovieId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Cinema>.ThrowIfNotFound(movie, "Không tìm thấy phim");


        return APIResponse<GetBookingResponse>.IsSuccess(
            new GetBookingResponse
            {
                Id = document.Id,
                MovieTitle = movie.Title,
                Total = document.Total,
                Seats = document.Seats,
                CinemaAddress = cinema.Address,
                CinemaName = cinema.Name,
                PaymentExpiredAt = document.PaymentExpiredAt,
                ShowTimeStartAt = showTime.StartAt,
                CreatedAt = document.CreatedAt,
            });
    }
}