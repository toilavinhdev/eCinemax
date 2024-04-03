using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Exceptions;
using MediatR;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.BookingCommands;

public class UpdateBookingExpiredCommand : IRequest;

public class UpdateBookingStatusCommandHandler(IMongoService mongoService) : IRequestHandler<UpdateBookingExpiredCommand>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task Handle(UpdateBookingExpiredCommand request, CancellationToken cancellationToken)
    {
        // TODO: Cập nhật các booking có trạng thái đang chờ thanh toán nhưng đã hết hạn
        
        var bookingFilterBuilder = Builders<Booking>.Filter;
        var bookingFilter = bookingFilterBuilder.Empty;
        bookingFilter &= bookingFilterBuilder.Eq(x => x.Status, BookingStatus.WaitForPay);
        bookingFilter &= bookingFilterBuilder.Lt(x => x.PaymentExpiredAt, DateTime.Now);

        var cursor = await _bookingCollection.FindAsync(bookingFilter, cancellationToken: cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            var batch = cursor.Current;
            foreach (var booking in batch)
            {
                var bookingUpdate = Builders<Booking>.Update.Set(x => x.Status, BookingStatus.Expired);
                await _bookingCollection.UpdateOneAsync(bookingFilter, bookingUpdate,
                    cancellationToken: cancellationToken);
                
                // TODO: Reset các reservation
                var showTimeFilter = Builders<ShowTime>.Filter.Eq(x => x.Id, booking.ShowTimeId);
                var showTime = await _showTimeCollection
                    .Find(showTimeFilter)
                    .FirstOrDefaultAsync(cancellationToken);
                DocumentNotFoundException<ShowTime>.ThrowIfNotFound(showTime, $"Lịch chiếu của hóa đơn {booking.Id} không tồn tại");

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
            }
        }
    }
}