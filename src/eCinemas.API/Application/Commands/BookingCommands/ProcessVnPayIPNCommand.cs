﻿using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Extensions;
using eCinemas.API.Shared.Library.VnPay;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.BookingCommands;

public class ProcessVnPayIPNCommand : VnPayResponse, IAPIRequest<ProcessVnPayIPNResponse>;

public class ProcessVnPayIPNCommandHandler(AppSettings appSettings,
    IMongoService mongoService) : IAPIRequestHandler<ProcessVnPayIPNCommand, ProcessVnPayIPNResponse>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse<ProcessVnPayIPNResponse>> Handle(ProcessVnPayIPNCommand request, CancellationToken cancellationToken)
    {
        var validate = request.IsValidSignature(appSettings.VnPayConfig.HashSecret);
        if (!validate) throw new BadRequestException("Signature không hợp lệ");

        var bookingId = request.vnp_TxnRef;
        var vnPayResponse = request.vnp_ResponseCode;
        var paymentTime = request.vnp_PayDate.ToLong();
        
        var bookingFilter = Builders<Booking>.Filter.Eq(x => x.Id, bookingId);
        var booking = await _bookingCollection.Find(bookingFilter).FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Payment>.ThrowIfNotFound(booking, $"Không tìm thấy đơn hàng {bookingId}");
        if (booking.Payment is null) throw new BadRequestException("Đơn hàng chưa có thông tin hóa đơn");
        
        var showTimeFilter = Builders<ShowTime>.Filter.Eq(x => x.Id, booking.ShowTimeId);
        var showTime = await _showTimeCollection.Find(showTimeFilter).FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<ShowTime>.ThrowIfNotFound(showTime, $"Không tìm thấy lịch chiếu");

        if (showTime.Status is ShowTimeStatus.NowShowing or ShowTimeStatus.Finished)
            throw new BadRequestException("Lịch chiếu đã ngừng bán vé");

        /*
         * 00: Success
         * Other: Failed
         */
        if (!vnPayResponse.Equals("00"))
        {
            // TODO: Update payment of booking status
            booking.Payment.Status = PaymentStatus.Failed;
            var bookingUpdate = Builders<Booking>.Update
                .Set(x => x.Status, BookingStatus.Failed)
                .Set(x => x.Payment, booking.Payment);
            await _bookingCollection.UpdateOneAsync(bookingFilter, bookingUpdate, cancellationToken: cancellationToken);

            // Todo: Update reservation status
            showTime.Reservations
                .SelectMany(x => x)
                .Where(x => booking.SeatNames.Any(seat => seat == x.Name))
                .ToList()
                .ForEach(reservation =>
                {
                    reservation.Status = ReservationStatus.Idle;
                    reservation.ReservationAt = null;
                    reservation.ReservationBy = null;
                });
            var showTimeUpdate = Builders<ShowTime>.Update
                .Set(x => x.Reservations, showTime.Reservations);
            await _showTimeCollection.UpdateOneAsync(showTimeFilter, showTimeUpdate,cancellationToken: cancellationToken);
            
            return APIResponse<ProcessVnPayIPNResponse>.IsSuccess(
                new ProcessVnPayIPNResponse
                { }, 
                $"Có lỗi xảy ra trong quá trình xử lý. Mã lỗi {vnPayResponse}");
        }
        else
        {
            // TODO: Update payment of booking status
            booking.Payment.Status = PaymentStatus.Success;
            booking.Payment.PaidAt = new DateTime(paymentTime);
            var bookingUpdate = Builders<Booking>.Update
                .Set(x => x.Status, BookingStatus.Success)
                .Set(x => x.Payment, booking.Payment);;
            await _bookingCollection.UpdateOneAsync(bookingFilter, bookingUpdate, cancellationToken: cancellationToken);
            
            // Todo: Update reservation status
            showTime.Reservations
                .SelectMany(x => x)
                .Where(x => booking.SeatNames.Any(seat => seat == x.Name))
                .ToList()
                .ForEach(reservation =>
                {
                    reservation.Status = ReservationStatus.SoldOut;
                });
            var showTimeUpdate = Builders<ShowTime>.Update
                .Set(x => x.Reservations, showTime.Reservations);
            await _showTimeCollection.UpdateOneAsync(showTimeFilter, showTimeUpdate,cancellationToken: cancellationToken);
            
            return APIResponse<ProcessVnPayIPNResponse>.IsSuccess(
                new ProcessVnPayIPNResponse
                { }, 
                "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ");
        }
    }
}