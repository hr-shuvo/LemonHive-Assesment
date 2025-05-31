using Microsoft.EntityFrameworkCore;

public class PaginatedList<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalPage { get; set; }
    public int TotalCount { get; set; }
    public List<T> DataList { get; set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        DataList = items;
        TotalCount = count;
        Page = pageIndex;
        TotalPage = (int)Math.Ceiling(count / (double)pageSize);
    }


    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}