using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Shared.SignalR;

public abstract class BaseSignalRHub(ConnectionManager connectionManager) : Hub
{
    public ConnectionManager ConnectionManager { get; set; } = connectionManager;
    
    protected string CurrentUserId => Context.UserIdentifier!;
    
    protected string CurrentConnectionId => Context.ConnectionId;
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        ConnectionManager.AddConnection(CurrentUserId, CurrentConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        ConnectionManager.RemoveConnection(CurrentUserId, CurrentConnectionId);
    }
}