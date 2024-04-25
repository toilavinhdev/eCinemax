using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Shared.SignalR;

public abstract class BaseSignalRHub(ConnectionManager connectionManager) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrEmpty(Context.UserIdentifier)) return;
        connectionManager.AddConnection(Context.UserIdentifier, Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (string.IsNullOrEmpty(Context.UserIdentifier)) return;
        connectionManager.RemoveConnection(Context.UserIdentifier, Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}