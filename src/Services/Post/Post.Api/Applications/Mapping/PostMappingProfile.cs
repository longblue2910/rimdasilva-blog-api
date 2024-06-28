using AutoMapper;
using Post.Api.Applications.Commands.Post;
using Post.Domain.Dtos.Post;

namespace Post.Api.Applications.Mapping;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<CreatePostCommand, CreateOrUpdatePostDto>();
    }
}
