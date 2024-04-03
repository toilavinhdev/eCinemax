using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.BookingCommands;

public class CreateBookingCommand : IAPIRequest
{
    public string ShowTimeId { get; set; } = default!;

    public List<string> SeatNames { get; set; } = default!;
}

public class CreateBookingCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CreateBookingCommand>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var showTimeFilterBuilder = Builders<ShowTime>.Filter;
        var showTimeFilter = showTimeFilterBuilder.Empty;
        showTimeFilter &= showTimeFilterBuilder.Eq(x => x.Id, request.ShowTimeId);
        showTimeFilter &= showTimeFilterBuilder.Eq(x => x.Status, ShowTimeStatus.Upcoming);
         var showTime = await _showTimeCollection
             .Find(showTimeFilter)
             .FirstOrDefaultAsync(cancellationToken);
         DocumentNotFoundException<ShowTime>.ThrowIfNotFound(showTime, "Không tìm thấy lịch chiếu");
         
         var total = 0;
         
         foreach (var seatName in request.SeatNames)
         {
             var reservation = showTime.Reservations
                 .SelectMany(x => x)
                 .FirstOrDefault(x => x.Name == seatName);
             
             if (reservation is null) throw new BadRequestException($"Không tìm thấy ghế ngồi {seatName}");
             if (reservation.Status == ReservationStatus.SoldOut)
                 throw new BadRequestException($"Ghế đã hết: {seatName}");
             
             reservation.Status = ReservationStatus.AwaitingPayment;
             reservation.ReservationAt = DateTime.Now;
             reservation.ReservationBy = mongoService.UserClaims().Id;
             total += showTime.Ticket.Find(x => x.Type == reservation.Type)?.Price
                      ?? throw new NullReferenceException("Lỗi hệ thống chưa cài đặt giá vé cho ghế này");
         }

         var booking = new Booking
         {
             Id = ObjectId.GenerateNewId().ToString(),
             ShowTimeId = showTime.Id,
             Total = total,
             SeatNames = request.SeatNames,
             Status = BookingStatus.WaitForPay,
             PaymentExpiredAt = DateTime.Now.AddMinutes(10) // Hết hạn thanh toán sau 10 phút, sử dụng job để track
         };
         booking.MarkCreated(mongoService.UserClaims().Id);
         await _bookingCollection.InsertOneAsync(booking, cancellationToken: cancellationToken);
        
         // Cập nhật trạng thái ghế đang chờ thanh toán của lịch chiếu
         showTime.Available = showTime.Reservations
             .Aggregate(0, 
                 (acc, cur) => acc + cur.Count(
                     x => x.Type != SeatType.Blank && 
                          x.Status == ReservationStatus.Idle));
         var showTimeUpdate = Builders<ShowTime>.Update
             .Set(x => x.Reservations, showTime.Reservations)
             .Set(x => x.Available, showTime.Available);
         await _showTimeCollection.UpdateOneAsync(
             showTimeFilter, 
             showTimeUpdate, 
             cancellationToken: cancellationToken);
         
         return APIResponse.IsSuccess();
    }
}
