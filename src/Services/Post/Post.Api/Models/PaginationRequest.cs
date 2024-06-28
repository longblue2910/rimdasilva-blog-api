namespace Post.Api.Models;

public record PaginationRequest(int PageSize = 10, int PageIndex = 1);
