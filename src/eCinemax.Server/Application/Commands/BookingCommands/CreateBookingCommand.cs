using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Persistence;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.BookingCommands;

public class CreateBookingCommand : IRequest<string>
{
    public string ShowTimeId { get; set; } = default!;

    public List<string> SeatNames { get; set; } = default!;
}

public class CreateBookingCommandHandler(
    IMongoService mongoService,
    IHubContext<ReservationHub> reservationHubContext) : IRequestHandler<CreateBookingCommand, string>
{
    private readonly IMongoCollection<Booking> _bookingCollection = mongoService.Collection<Booking>();
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private const int BookingDurationInMinutes = 5; // Hết hạn thanh toán sau 10 phút, sử dụng job để track
    
    public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
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

         var bookingSeats = new List<BookingSeat>();
         
         foreach (var seatName in request.SeatNames)
         {
             var reservation = showTime.Reservations
                 .SelectMany(x => x)
                 .FirstOrDefault(x => x.Name == seatName);
             
             if (reservation is null) throw new BadRequestException($"Không tìm thấy ghế ngồi {seatName}");
             if (reservation.Status == ReservationStatus.SoldOut)
                 throw new BadRequestException($"Ghế đã hết: {seatName}");
             if (reservation.Status == ReservationStatus.AwaitingPayment)
                 throw new BadRequestException($"Ghế đã được đặt: {seatName}");
             
             reservation.Status = ReservationStatus.AwaitingPayment;
             reservation.ReservationAt = DateTime.Now;
             reservation.ReservationBy = mongoService.UserClaims().Id;
             total += showTime.Ticket.Find(x => x.Type == reservation.Type)?.Price
                      ?? throw new NullReferenceException("Lỗi hệ thống chưa cài đặt giá vé cho ghế này");

             var bookingSeat = bookingSeats.FirstOrDefault(x => x.Type == reservation.Type);
             if (bookingSeat is not null)
             {
                 bookingSeat.SeatNames.Add(reservation.Name);
             }
             else
             {
                 bookingSeats.Add(new BookingSeat
                 {
                     Type = reservation.Type,
                     SeatNames = [reservation.Name],
                 });
             }
         }

         var booking = new Booking
         {
             Id = ObjectId.GenerateNewId().ToString(),
             ShowTimeId = showTime.Id,
             MovieId = showTime.MovieId,
             Total = total,
             Seats = bookingSeats,
             Status = BookingStatus.WaitForPay,
             PaymentExpiredAt = DateTime.Now.AddMinutes(BookingDurationInMinutes)
         };
         booking.MarkCreated(mongoService.UserClaims().Id);
         await _bookingCollection.InsertOneAsync(booking, cancellationToken: cancellationToken);
        
         // Cập nhật trạng thái ghế đang chờ thanh toán của lịch chiếu
         var showTimeUpdate = Builders<ShowTime>.Update
             .Set(x => x.Reservations, showTime.Reservations);
         await _showTimeCollection.UpdateOneAsync(
             showTimeFilter, 
             showTimeUpdate, 
             cancellationToken: cancellationToken);

         await reservationHubContext.Clients.Group(showTime.Id).SendAsync(
             "ReceivedSeatsAwaitingPayment",
             request.SeatNames.ToArray(),
             cancellationToken);
         
         return booking.Id;
    }
}
