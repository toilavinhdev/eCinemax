using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Shared.SignalR;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection) => connection.User.FindFirst("id")!.Value;
}