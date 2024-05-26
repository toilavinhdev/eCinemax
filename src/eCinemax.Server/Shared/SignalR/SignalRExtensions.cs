using Microsoft.AspNetCore.SignalR;

namespace eCinemax.Server.Shared.SignalR;

public static class SignalRExtensions
{
    public static IServiceCollection AddSignalRManager(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddTransient<ConnectionManager>();
        services.AddSingleton<IUserIdProvider, UserIdProvider>();
        return services;
    }
}