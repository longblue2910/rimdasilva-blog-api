public class PaginatedItems<TEntity> where TEntity : class
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public long Count { get; }
    public IEnumerable<TEntity> Data { get; }
    public bool HasPrev { get; }
    public bool HasNext { get; }

    public PaginatedItems(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
        HasPrev = PageIndex > 1;
        HasNext = PageIndex < (int)Math.Ceiling((double)Count / PageSize);
    }
}