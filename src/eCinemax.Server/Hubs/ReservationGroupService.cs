using System.Collections.Concurrent;

namespace eCinemax.Server.Hubs;

public class ReservationGroupService
{
    /// <summary>
    /// Key: ShowtimeId
    /// Value: Dictionary(ConnectionId, HashSet(SeatNames))
    /// </summary>
    public ConcurrentDictionary<string, Dictionary<string, HashSet<string>>?> Groups = new();

    public void JoinShowTime(string showtimeId, string connectionId)
    {
        var existed = Groups.GetValueOrDefault(showtimeId);
        if (existed is null)
        {
            Groups.TryAdd(showtimeId,
                new Dictionary<string, HashSet<string>>
                {
                    {
                        connectionId, []
                    }
                });
        }
        else
        {
            existed.TryAdd(connectionId, []);
        }
    }

    public void LeaveShowTime(string showtimeId, string connectionId)
    {
        var existed = Groups.GetValueOrDefault(showtimeId);
        if (existed is null) return;
        existed.Remove(connectionId);
        if (existed.Count == 0) Groups.TryRemove(showtimeId, out _);
    }

    public IEnumerable<string> GetSelectedSeats(string showtimeId)
    {
        return Groups.GetValueOrDefault(showtimeId)?.SelectMany(x => x.Value) ?? [];
    }

    public void SelectSeat(string showtimeId, string connectionId, string seatName)
    {
        var showtime = Groups.GetValueOrDefault(showtimeId);
        var selectedSeats = showtime?.GetValueOrDefault(connectionId);
        if (showtime is null || selectedSeats is null) return;
        
        // TODO: Check nếu ghế được chọn bởi connection khác -> do noting
        var idOfSelectedSeatConnection = showtime
            .Where(pair => pair.Value.Contains(seatName))
            .Select(pair => pair.Key)
            .FirstOrDefault();
        if (idOfSelectedSeatConnection is not null && idOfSelectedSeatConnection != connectionId) return;

        if (selectedSeats.Any(x => x == seatName))
        {
            selectedSeats.Remove(seatName);
        }
        else
        {
            selectedSeats.Add(seatName);
        }
    }
}