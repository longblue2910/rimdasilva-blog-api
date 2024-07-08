using AutoMapper;
using Contracts.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Domain.Exceptions;
using Post.Infrastructure;

namespace Post.Api.Applications.Commands.Category;

public class CreateCatoryCommandHandler(PostDbContext context, IMapper mapper) : IRequestHandler<CreateCategoryCommand, bool>
{
    private readonly PostDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        //Check exist
        var exist = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == request.Slug, 
            cancellationToken: cancellationToken);

        if (exist != null)
            throw new PostDomainException("Category đã tồn tại.");

        var categoryEntity = _mapper.Map<Domain.AggregatesModel.CategoryAggregate.Category>(request);
        _context.Categories.Add(categoryEntity);

        #region Upload image

        if (request?.File?.Length > 0)
        {
            string uploadImagesFolder = RootPathConfig.UploadPath.PathImagePost;
            if (!Directory.Exists(uploadImagesFolder))
                Directory.CreateDirectory(uploadImagesFolder);

            string extension = Path.GetExtension(request.File.FileName);

            string imageName = $"{request.Slug}{extension}";
            string filePathImage = Path.Combine(uploadImagesFolder, imageName);

            using (FileStream stream = new(filePathImage, FileMode.Create))
            {
                await request.File.CopyToAsync(stream, cancellationToken);
            }

            categoryEntity.ImgUrl = imageName;
        }

        #endregion

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
