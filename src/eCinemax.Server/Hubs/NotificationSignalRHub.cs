using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Hubs;

public class NotificationSignalRHub(ConnectionManager connectionManager, ILogger<NotificationSignalRHub> logger) : BaseSignalRHub(connectionManager)
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        logger.LogInformation("{Hub}: Hello world!, User {Id} connected on Hub {Connection}", 
            nameof(NotificationSignalRHub), Context.UserIdentifier, Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        logger.LogInformation("{Hub}: User {Id} disconnected on Hub {Connection}", 
            nameof(NotificationSignalRHub), Context.UserIdentifier, Context.ConnectionId);
    }
    
    public Task SendMessageBroadcast(string name, string message)
    {
        logger.LogInformation("On Send Message {0}: {1}", name, message);
        return Clients.All.SendAsync("SendMessageBroadcast", name, message);
    }
}