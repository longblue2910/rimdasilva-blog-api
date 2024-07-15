namespace Contracts.Common.Responses;

public class ApiPagingSuccessResponse : ApiSuccessResponse
{
    public PagingResponse Paging { get; set; } = new();
}

public class PagingResponse
{
    public int TotalCount { get; set; }
    public int TotalPage { get; set; }
    public int PageSize { get; set; }
}
