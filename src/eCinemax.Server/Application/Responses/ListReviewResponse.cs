namespace eCinemax.Server.Application.Responses;

public class ListReviewResponse(
    List<ReviewViewModel>
        records,
    int pageIndex,
    int pageSize,
    int totalRecord) : PaginationResponse<ReviewViewModel>(records, pageIndex, pageSize, totalRecord)
{
    public ReviewViewModel? UserReview { get; set; }
}

public class ReviewViewModel
{
    public string Id { get; set; } = default!;
    
    public int Rate { get; set; }
    
    public string User { get; set; } = default!;
    
    public string? Review { get; set; }
    
    public DateTime CreatedAt { get; set; }
}