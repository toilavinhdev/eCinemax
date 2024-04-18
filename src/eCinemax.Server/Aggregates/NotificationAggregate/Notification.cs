using eCinemax.Server.Shared.ValueObjects;

namespace eCinemax.Server.Aggregates.NotificationAggregate;

public class Notification : TimeTrackingDocument, IAggregateRoot
{
    public string Title { get; set; } = default!;
    
    public string Content { get; set; } = default!;

    public string PhotoUrl { get; set; } = default!;
}