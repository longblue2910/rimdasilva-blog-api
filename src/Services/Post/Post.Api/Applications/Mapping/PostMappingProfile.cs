using AutoMapper;
using Post.Api.Applications.Commands.Post;

namespace Post.Api.Applications.Mapping;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<CreatePostCommand, Domain.AggregatesModel.PostAggregate.Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
