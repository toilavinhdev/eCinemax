using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Persistence;
using eCinemax.Server.Shared.Library.VnPay;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.BookingCommands;

public class ProcessVnPayReturnCommand : VnPayResponse, IAPIRequest;

public class ProcessVnPayReturnCommandHandler(AppSettings appSettings, IMongoService mongoService) : IAPIRequestHandler<ProcessVnPayReturnCommand>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse> Handle(ProcessVnPayReturnCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValidSignature(appSettings.VnPayConfig.HashSecret)) 
            throw new BadRequestException("Signature không hợp lệ");

        var bookingId = request.vnp_TxnRef;
        var vnPayResponse = request.vnp_ResponseCode;
        
        var payment = await _bookingCollection.Find(x => x.Id == bookingId).FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Payment>.ThrowIfNotFound(payment, $"Không tìm thấy đơn hàng {bookingId}");

        /*
         * 00: Success
         * Other: Failed
         */
        if (!vnPayResponse.Equals("00"))
        {
            return APIResponse.IsSuccess("Có lỗi xảy ra trong quá trình xử lý hóa đơn");

        }
        else
        {
            return APIResponse.IsSuccess("Thanh toán thành công hóa đơn");
        }
    }
}