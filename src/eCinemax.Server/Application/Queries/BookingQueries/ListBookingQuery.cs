using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.BookingQueries;

public class ListBookingQuery : IAPIRequest<ListBookingResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public BookingStatus Status { get; set; }
    
    internal class ListBookingQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListBookingQuery, ListBookingResponse>
    {
        private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
        
        private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
        
        private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
        
        public async Task<APIResponse<ListBookingResponse>> Handle(ListBookingQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = mongoService.UserClaims().Id;
            
            var builder = Builders<Booking>.Filter;
            var filter = builder.Empty;
            filter &= builder.Eq(x => x.CreatedBy, currentUserId);
            filter &= builder.Eq(x => x.Status, request.Status);

            var aggregate = await _bookingCollection
                .Aggregate()
                .Match(filter)
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Limit(request.PageSize)
                .Lookup<Booking, ShowTime, BookingWithShowTimes>(
                    _showTimeCollection,
                    b => b.ShowTimeId,
                    s => s.Id,
                    booking => booking.ShowTimes)
                .Lookup<BookingWithShowTimes, Movie, BookingWithMovie>(
                    _movieCollection,
                    b => b.MovieId,
                    m => m.Id,
                    booking => booking.Movie)
                .ToListAsync(cancellationToken);
            
            var totalRecord = await _bookingCollection.CountDocumentsAsync(filter, cancellationToken:cancellationToken);
            var data = aggregate.Select(x => new BookingViewModel
                {
                    Id = x.Id,
                    Total = x.Total,
                    Seats = x.Seats,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    MovieId = x.Movie.FirstOrDefault()!.Id,
                    MovieName = x.Movie.FirstOrDefault()!.Title,
                    MoviePosterUrl = x.Movie.FirstOrDefault()!.PosterUrl
                }).ToList();

            return APIResponse<ListBookingResponse>.IsSuccess(new ListBookingResponse(
                data, 
                request.PageIndex,
                request.PageSize, 
                (int)totalRecord));
        }
    }
    
    private class BookingWithShowTimes : Booking
    {
        public List<ShowTime>? ShowTimes { get; set; }
    }


    private class BookingWithMovie : BookingWithShowTimes
    {
        public List<Movie> Movie { get; set; } = default!;
    }
}

public class ListBookingQueryValidator : AbstractValidator<ListBookingQuery>
{
    public ListBookingQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Status).NotNull();
    }
}