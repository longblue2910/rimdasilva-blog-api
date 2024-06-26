using MediatR;

namespace Post.Api.Applications.Queries.Category;

public class GetsCategoryQuery : IRequest<List<Post.Domain.AggregatesModel.CategoryAggregate.Category>>
{
}
