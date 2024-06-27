namespace Post.Api.Applications.Queries.Post;

public interface IPostQueries
{
    Task<Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id);
    Task<Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug);
}
