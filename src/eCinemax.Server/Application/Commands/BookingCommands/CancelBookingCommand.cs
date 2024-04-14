using System.Threading;
using System.Threading.Tasks;
using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.BookingCommands;

public class CancelBookingCommand : IAPIRequest
{
    public string Id { get; set; } = default!;
}

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class CancelBookingCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CancelBookingCommand>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        // TODO: Cập nhật trạng thái booking & reset reservations của showtime
        
        var bookingFilter = Builders<Booking>.Filter.Eq(x => x.Id, request.Id);
        var booking = await _bookingCollection.Find(bookingFilter).FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Booking>.ThrowIfNotFound(booking, "Đơn hàng không tồn tại");

        var showTimeFilter = Builders<ShowTime>.Filter.Eq(x => x.Id, booking.ShowTimeId);
        var showTime = await _showTimeCollection.Find(showTimeFilter).FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Booking>.ThrowIfNotFound(showTime, "Lịch chiếu không tồn tại");
        
        booking.Status = BookingStatus.Cancelled;
        booking.MarkModified(mongoService.UserClaims().Id);
        var bookingUpdate = Builders<Booking>.Update
            .Set(x => x.Status, booking.Status)
            .Set(x => x.ModifiedAt, booking.ModifiedAt)
            .Set(x => x.ModifiedBy, booking.ModifiedBy);
        await _bookingCollection.UpdateOneAsync(bookingFilter, bookingUpdate, cancellationToken: cancellationToken);
        
        showTime.Reservations
            .SelectMany(x => x)
            .Where(x => x.ReservationBy == booking.CreatedBy && x.Status == ReservationStatus.AwaitingPayment)
            .ToList()
            .ForEach(reservation =>
            {
                reservation.Status = ReservationStatus.Idle;
                reservation.ReservationAt = null;
                reservation.ReservationBy = null;
            });
        var showTimeUpdate = Builders<ShowTime>.Update
            .Set(x => x.Reservations, showTime.Reservations);
        await _showTimeCollection.UpdateOneAsync(
            showTimeFilter, 
            showTimeUpdate,
            cancellationToken: cancellationToken);
        
        return APIResponse.IsSuccess("Hoàn tác thành công");
    }
}