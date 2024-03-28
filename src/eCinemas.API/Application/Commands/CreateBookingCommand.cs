using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands;

public class CreateBookingCommand : IAPIRequest<Booking>
{
    public string ShowTimeId { get; set; } = default!;
    
    public List<Seat> Seats { get; set; } = default!;
}

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.ShowTimeId).NotEmpty();
        RuleFor(x => x.Seats).NotEmpty();
    }
}

public class CreateBookingCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CreateBookingCommand, Booking>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse<Booking>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = mongoService.GetUserClaimValue()?.Id;
        if (currentUserId is null) throw new NullReferenceException("User Id is required.");
        
        var showTime = await _showTimeCollection
            .Find(x => x.Id == request.ShowTimeId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<ShowTime>.ThrowIfNotFound(showTime, request.ShowTimeId);

        if (showTime.StartAt < DateTimeOffset.Now)
            throw new ApplicationException("Quá thời gian đặt vé.");

        var document = new Booking
        {
            ShowTime = request.ShowTimeId,
            Seats = request.Seats,
            Total = request.Seats.Aggregate(0, (acc, cur) =>
            {
                var seat = showTime.Ticket.FirstOrDefault(x => x.Type == cur.Type);
                if (seat is null) throw new ArgumentException("Invalid seat price.");
                return acc + seat.Price;
            })
        };
        document.MarkCreated(currentUserId);
        await _bookingCollection.InsertOneAsync(document, cancellationToken:cancellationToken);
        
        // update showtime reservations
        request.Seats.ForEach(seat =>
        {
            // showTime.Reservations.ForEach(row =>
            // {
            //     // var reservation = row.Find(x => x.Row == seat.Row && x.Column == seat.Column);
            //     var reservation = Seat.GetSeat(showTime.Reservations, seat.Row.ToCharArray()[0], seat.Column);
            //     if (reservation is null) throw new NullReferenceException("Seat name is invalid");
            //     if (reservation.Status == SeatStatus.SoldOut) throw new BadRequestException("Seat is sold out.");
            //     reservation.MarkSoldOut();
            // });
        });
        showTime.Available -= request.Seats.Count;
        var showTimeFilter = Builders<ShowTime>.Filter.Eq(x => x.Id, request.ShowTimeId);
        var showTimeUpdate = Builders<ShowTime>.Update
            .Set(x => x.Available, showTime.Available)
            .Set(x => x.Reservations, showTime.Reservations);
        await _showTimeCollection.UpdateOneAsync(
            showTimeFilter,
            showTimeUpdate,
            cancellationToken: cancellationToken);

        return APIResponse<Booking>.IsSuccess(document);
    }
}