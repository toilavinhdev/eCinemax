using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Hubs;

public class ReservationHub(ConnectionManager connectionManager, 
    ReservationGroupService reservationGroupService,
    ILogger<NotificationHub> logger) : BaseSignalRHub(connectionManager)
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Groups.AddToGroupAsync(CurrentConnectionId, GetShowTimeIdFromQuery());
        reservationGroupService.JoinShowTime(GetShowTimeIdFromQuery(), CurrentConnectionId);
        await SendSelectedSeatsOnGroup();
        
        logger.LogInformation("{Hub}: User {Id} connected on Hub {Connection}", 
            nameof(ReservationHub), CurrentUserId, CurrentConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        await Groups.RemoveFromGroupAsync(CurrentConnectionId, GetShowTimeIdFromQuery());
        reservationGroupService.LeaveShowTime(GetShowTimeIdFromQuery(), CurrentConnectionId);
        await SendSelectedSeatsOnGroup();

        logger.LogInformation("{Hub}: User {Id} disconnected on Hub {Connection}", 
            nameof(ReservationHub), CurrentUserId, CurrentConnectionId);
    }

    public Task OnTouchSeat(string seatName)
    {
        reservationGroupService.SelectSeat(GetShowTimeIdFromQuery(), CurrentConnectionId, seatName);
        return SendSelectedSeatsOnGroup();
    }
    
    private Task SendSelectedSeatsOnGroup()
    {
        // TODO: Receiver các ghế đang chọn cho all client trong group(showtime)
        var selectedSeats = reservationGroupService.GetSelectedSeats(GetShowTimeIdFromQuery());
        return Clients
            .Groups(GetShowTimeIdFromQuery())
            .SendAsync(nameof(OnTouchSeat), selectedSeats);
    }

    private string GetShowTimeIdFromQuery()
    {
        var httpContext = Context.GetHttpContext();
        return httpContext?.Request.Query["showTimeId"].ToString()!;
    }
}