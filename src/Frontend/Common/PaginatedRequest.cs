namespace Frontend.Common;

public class PaginatedRequest
{
    public string? Query { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}