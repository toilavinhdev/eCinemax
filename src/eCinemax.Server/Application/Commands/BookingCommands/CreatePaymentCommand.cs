using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Helpers;
using eCinemax.Server.Persistence;
using eCinemax.Server.Shared.Library.VnPay;
using MongoDB.Driver;
using static System.String;

namespace eCinemax.Server.Application.Commands.BookingCommands;

public class CreatePaymentCommand : IAPIRequest<CreatePaymentResponse>
{
    public string BookingId { get; set; } = default!;
    
    public PaymentDestination Destination { get; set; }
}

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotNull().WithMessage("Vui lòng chọn hóa đơn");
        RuleFor(x => x.Destination)
            .NotNull().WithMessage("Vui lòng chọn phương thức thanh toán");
    }
}

public class CreatePaymentCommandHandler(
    IMongoService mongoService,
    ILogger<CreatePaymentCommandHandler> logger,
    ReservationHub reservationHub,
    AppSettings appSettings) : IAPIRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse<CreatePaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = mongoService.UserClaims().Id;
        var user = await _userCollection
            .Find(x => x.Id == currentUserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        var bookingFilter = Builders<Booking>.Filter.Eq(x => x.Id, request.BookingId);
        var booking = await _bookingCollection
            .Find(bookingFilter)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Booking>.ThrowIfNotFound(booking, "Không tìm thấy hóa đơn");

        if (booking.PaymentExpiredAt < DateTime.Now || booking.Status == BookingStatus.Expired)
            throw new BadRequestException("Hóa đơn đã hết hạn thanh toán");
        
        // TODO: Create payment for booking
        booking.Payment = new Payment
        {
            Destination = request.Destination,
            Content = $"Thanh toán đơn hàng {booking.Id}",
            Amount = booking.Total,
            PaidAt = DateTime.Now,
            Status = PaymentStatus.Success // để tạm success test trước, sau rồi triển khai VNPay(PaymentStatus.Processing)
        };
        booking.Status = BookingStatus.Success;
        
        // TODO: Update reservation showtime
        var showTimeFilter = Builders<ShowTime>.Filter.Eq(x => x.Id, booking.ShowTimeId);
        var showTime = await _showTimeCollection
            .Find(showTimeFilter)
            .FirstOrDefaultAsync(cancellationToken);
        if (showTime is null) throw new BadRequestException("Lịch chiếu không tồn tại. Vui lòng thử lại");
        
        var reservations = showTime.Reservations
            .SelectMany(x => x)
            .Where(x => 
                x.ReservationBy == currentUserId && 
                booking.Seats.SelectMany(seat => seat.SeatNames).Contains(x.Name))
            .ToList();
        if (reservations is null || reservations.Count == 0) throw new BadRequestException("Có lỗi xảy ra");

        foreach (var reservation in reservations)
        {
            reservation.Status = ReservationStatus.SoldOut;
        }

        try
        {
            var bookingUpdate = Builders<Booking>.Update
                .Set(x => x.Status, booking.Status)
                .Set(x => x.Payment, booking.Payment);
            await _bookingCollection.UpdateOneAsync(bookingFilter, bookingUpdate, cancellationToken: cancellationToken);

            var showTimeUpdate = Builders<ShowTime>.Update.Set(x => x.Reservations, showTime.Reservations);
            await _showTimeCollection.UpdateOneAsync(showTimeFilter,showTimeUpdate , cancellationToken:cancellationToken);
        }
        catch (Exception _)
        {
            var message = $"Giao dịch thất bại. Mã hóa đơn giao dịch {booking.Id}";
            logger.LogError($"{message}: {_}");
            throw new BadRequestException(message);
        }

        // await reservationHub.SendSeatsSoldOut(reservations.Select(x => x.Name).ToArray());

        // await EmailHelper.SmptSendAsync(
        //     appSettings.GmailConfig,
        //     user.Email,
        //     "Thanh toán thành công",
        //     "Thanh toán thành công đơn hàng");
        
        return APIResponse<CreatePaymentResponse>.IsSuccess(
            new CreatePaymentResponse
            {
                RedirectUrl = BuildPaymentUrl(request, booking.Id, booking.Total),
            });
    }

    private string BuildPaymentUrl(CreatePaymentCommand request, string bookingId, int amount)
    {
        var paymentUrl = Empty;
        
        switch (request.Destination)
        {
            case PaymentDestination.VnPay:
                var vnPayRequest = new VnPayRequest
                {
                    vnp_Version = appSettings.VnPayConfig.Version,
                    vnp_TmnCode = appSettings.VnPayConfig.TmnCode,
                    vnp_ReturnUrl = appSettings.Host + appSettings.VnPayConfig.ReturnEndpoint,
                    vnp_Command = "pay",
                    vnp_Amount = amount * 100,
                    vnp_BankCode = null,
                    vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    vnp_CurrCode = "VND",
                    vnp_IpAddr = IPExtensions.GetLocalIPAddress(),
                    vnp_Locale = "vn",
                    vnp_OrderInfo = $"Thanh toán đơn hàng {bookingId}",
                    vnp_OrderType = "other",
                    vnp_TxnRef = bookingId
                };
                
                paymentUrl = vnPayRequest.CreateRequestUrl(
                    appSettings.VnPayConfig.Url, 
                    appSettings.VnPayConfig.HashSecret);
                break;
            case PaymentDestination.Momo:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return paymentUrl;
    }
}