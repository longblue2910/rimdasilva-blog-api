using MediatR;

namespace Post.Api.Applications.Queries.Category;

public class GetsCategoryQuery : IRequest<List<Domain.AggregatesModel.CategoryAggregate.Category>>
{
}
