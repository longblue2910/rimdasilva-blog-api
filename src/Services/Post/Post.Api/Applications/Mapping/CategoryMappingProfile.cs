using AutoMapper;
using Post.Api.Applications.Commands.Category;
using Post.Domain.AggregatesModel.CategoryAggregate;

namespace Post.Api.Applications.Mapping;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CreateCategoryCommand, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}