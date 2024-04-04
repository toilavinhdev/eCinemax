using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Library.VnPay;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

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

public class CreatePaymentCommandHandler(IMongoService mongoService, 
    AppSettings appSettings) : IAPIRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse<CreatePaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
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
            PaidAt = null,
            Status = PaymentStatus.Processing // để tạm success test trước, sau rồi triển khai VNPay(PaymentStatus.Processing)
        };

        var bookingUpdate = Builders<Booking>.Update.Set(x => x.Payment, booking.Payment);
        await _bookingCollection.UpdateOneAsync(bookingFilter, bookingUpdate, cancellationToken: cancellationToken);
        
        return APIResponse<CreatePaymentResponse>.IsSuccess(
            new CreatePaymentResponse
            {
                RedirectUrl = BuildPaymentUrl(request, booking.Id, booking.Total),
            });
    }

    private string BuildPaymentUrl(CreatePaymentCommand request, string bookingId, int amount)
    {
        var paymentUrl = string.Empty;
        
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
                    vnp_IpAddr = mongoService.IpAddress() ?? string.Empty,
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