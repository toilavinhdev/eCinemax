namespace eCinemax.Server.Shared.ValueObjects;

public class Pagination(int pageIndex, int pageSize, int totalRecord)
{
    public int PageIndex { get; set; } = pageIndex;

    public int PageSize { get; set; } = pageSize;

    public int TotalRecord { get; set; } = totalRecord;

    public int TotalPage => (int)Math.Ceiling(TotalRecord / (double)PageSize);

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPage;
}

public interface IPaginationRequest
{
    int PageIndex { get; set; }
    
    int PageSize { get; set; }
}

public class Pagination<T>(List<T> records, int pageIndex, int pageSize, int totalRecord) : Pagination(pageIndex, pageSize, totalRecord)
{
    public List<T> Records { get; set; } = records;
}

public abstract class PaginationResponse<T>(List<T> records, int pageIndex, int pageSize, int totalRecord)
{
    public Pagination Pagination { get; set; } = new(pageIndex, pageSize, totalRecord);

    public List<T> Records { get; set; } = records;
}