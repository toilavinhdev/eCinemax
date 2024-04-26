using eCinemax.Server.Aggregates.BookingAggregate;

namespace eCinemax.Server.Application.Responses;

public class ListBookingResponse(
    List<BookingViewModel> records,
    int pageIndex,
    int pageSize,
    int totalRecord) : PaginationResponse<BookingViewModel>(records, pageIndex, pageSize, totalRecord);

public class BookingViewModel
{
    public string Id { get; set; } = default!;
    
    public List<BookingSeat> Seats { get; set; } = default!;    
    public int Total { get; set; }
    
    public BookingStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string MovieId { get; set; } = default!;
    
    public string MovieName { get; set; } = default!;

    public string MoviePosterUrl { get; set; } = default!;
}