using eCinemax.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Hubs;

public class ReservationHub(ConnectionManager connectionManager,
    ReservationGroupService reservationGroupService,
    ILogger<NotificationHub> logger) : BaseSignalRHub(connectionManager)
{
    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrEmpty(GetShowTimeId()))
        {
            Context.Abort();
            return;
        };
        await base.OnConnectedAsync();
        await Groups.AddToGroupAsync(CurrentConnectionId, GetShowTimeId());
        reservationGroupService.JoinShowTime(GetShowTimeId(), CurrentConnectionId);
        await SendSelectedSeatsOnGroup();
        
        logger.LogInformation("{Hub}: UserId {Id} connected on ConnectionId {Connection} joined GroupId {Group}", 
            nameof(ReservationHub), CurrentUserId, CurrentConnectionId, GetShowTimeId());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        await Groups.RemoveFromGroupAsync(CurrentConnectionId, GetShowTimeId());
        reservationGroupService.LeaveShowTime(GetShowTimeId(), CurrentConnectionId);
        await SendSelectedSeatsOnGroup();

        logger.LogInformation("{Hub}: User {Id} disconnected on Hub {Connection} leaved GroupId {Group}", 
            nameof(ReservationHub), CurrentUserId, CurrentConnectionId, GetShowTimeId());
    }

    public Task OnTouchSeat(string seatName)
    {
        reservationGroupService.SelectSeat(GetShowTimeId(), CurrentConnectionId, seatName);
        return SendSelectedSeatsOnGroup();
    }

    public Task SendSeatsAwaitingPayment(string[] seatNames)
    {
        return Clients
            .Groups(GetShowTimeId())
            .SendAsync(nameof(SendSeatsAwaitingPayment), seatNames);
    }
    
    public Task SendSeatsSoldOut(string[] seatNames)
    {
        return Clients
            .Groups(GetShowTimeId())
            .SendAsync(nameof(SendSeatsSoldOut), seatNames);
    }
    
    private Task SendSelectedSeatsOnGroup()
    {
        // TODO: Receiver các ghế đang chọn cho all client trong group(showtime)
        var selectedSeats = reservationGroupService.GetSelectedSeats(GetShowTimeId());
        return Clients
            .Groups(GetShowTimeId())
            .SendAsync(nameof(SendSelectedSeatsOnGroup), selectedSeats);
    }

    private string GetShowTimeId()
    {
        var httpContext = Context.GetHttpContext();
        var id = httpContext?.Request.Headers["showTimeId"].ToString();
        return id!;
    }
}