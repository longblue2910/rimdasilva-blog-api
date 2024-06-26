using AutoMapper;
using Post.Api.Applications.Commands.User;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Api.Applications.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
