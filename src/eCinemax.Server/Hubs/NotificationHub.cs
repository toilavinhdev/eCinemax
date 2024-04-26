using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Hubs;

public class NotificationHub(ConnectionManager connectionManager, ILogger<NotificationHub> logger) : BaseSignalRHub(connectionManager)
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        logger.LogInformation("{Hub}: Hello world!, User {Id} connected on Hub {Connection}", 
            nameof(NotificationHub), CurrentUserId, CurrentConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        logger.LogInformation("{Hub}: User {Id} disconnected on Hub {Connection}", 
            nameof(NotificationHub), CurrentUserId, CurrentConnectionId);
    }
    
    public Task SendMessageBroadcast(string name, string message)
    {
        logger.LogInformation("On Send Message {0}: {1}", name, message);
        return Clients.All.SendAsync(nameof(SendMessageBroadcast), name, message);
    }
}