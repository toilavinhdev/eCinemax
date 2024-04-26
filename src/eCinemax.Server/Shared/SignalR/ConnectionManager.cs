using System.Collections.Concurrent;

namespace eCinemax.Server.Shared.SignalR;

public class ConnectionManager
{
    public ConcurrentDictionary<string, HashSet<string>> Connections { get; set; } = new();
    
    public IEnumerable<string> GetConnectionIds(string userId)
    {
        return Connections.TryGetValue(userId, out var connections) ? connections : [];
    }

    public void AddConnection(string userId, string connectionId)
    {
        var existed = Connections.GetValueOrDefault(userId);
        if (existed is null)
        {
            existed = [];
            Connections.TryAdd(userId, existed);
        }
        existed.Add(connectionId);
    }

    public void RemoveConnection(string userId, string connectionId)
    {
        var existed = Connections.GetValueOrDefault(userId);
        if (existed is null) return;
        existed.Remove(connectionId);
        if (existed.Count == 0) Connections.TryRemove(userId, out _);
    }
}