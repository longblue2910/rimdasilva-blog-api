using MediatR;
using Post.Domain.AggregatesModel.CategoryAggregate;

namespace Post.Api.Applications.Queries.Category;

public class GetsCategoryQueryHandler(ICategoryRepository repository) : IRequestHandler<GetsCategoryQuery, List<Domain.AggregatesModel.CategoryAggregate.Category>>
{
    private readonly ICategoryRepository _repository = repository;

    public async Task<List<Domain.AggregatesModel.CategoryAggregate.Category>> Handle(GetsCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetCategories(request.Size);
    }
}
