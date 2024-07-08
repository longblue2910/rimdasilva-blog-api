using MediatR;

namespace Post.Api.Applications.Commands.Category;

public class CreateCategoryCommand : IRequest<bool>
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public IFormFile File { get; set; }
    public int OrderIndex { get; set; }
    public string TagName { get; set; }
}
