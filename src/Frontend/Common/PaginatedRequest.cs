namespace Frontend.Common;

public class PaginatedRequest
{
    private const int MaxPageSize = 2;
    
    public string? Query { get; set; }
    private int _page = 1;
    private int _pageSize = MaxPageSize;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? MaxPageSize : value;
    }
}