using eCinemax.Server.Shared.ValueObjects;

namespace eCinemax.Server.Application.Responses;

public class ListNotificationResponse(List<NotificationViewModel> records, 
    int pageIndex, int pageSize, int totalRecord) : PaginationResponse<NotificationViewModel>(records, pageIndex, pageSize, totalRecord);

public class NotificationViewModel
{
    public string Id { get; set; } = default!;
    
    public string Title { get; set; } = default!;
    
    public string Content { get; set; } = default!;

    public string PhotoUrl { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
}