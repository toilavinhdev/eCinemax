using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Application.Responses;

public class ListShowTimeResponse(List<ShowTimeListView> records, int pageIndex, 
    int pageSize, int totalRecord) : PaginationResponse<ShowTimeListView>(records, pageIndex, pageSize, totalRecord)
{
    
}

public class ShowTimeListView
{
    public string Id { get; set; } = default!;

    public string MovieId { get; set; } = default!;
    
    public DateTimeOffset StartAt { get; set; }
}