using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Shared.SignalR;

public abstract class BaseSignalRHub(ConnectionManager connectionManager) : Hub
{
    protected string CurrentUserId => Context.UserIdentifier!;
    
    protected string CurrentConnectionId => Context.ConnectionId;
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        connectionManager.AddConnection(CurrentUserId, CurrentConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        connectionManager.RemoveConnection(CurrentUserId, CurrentConnectionId);
    }
}