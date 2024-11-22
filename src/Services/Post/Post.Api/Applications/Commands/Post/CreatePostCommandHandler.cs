using AutoMapper;
using Contracts.Configurations;
using Contracts.Exceptions;
using MediatR;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Commands.Post;

public class CreatePostCommandHandler(IPostRepository repository, IMapper mapper) : IRequestHandler<CreatePostCommand, bool>
{
    private readonly IPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
         var postEntity = _mapper.Map<Domain.AggregatesModel.PostAggregate.Post>(request);
        postEntity.CountWatch = 0;

        // Kiểm tra nếu title đã tồn tại
        var existingPost = await _repository.FindBySlugAsync(request.Slug);
        if (existingPost != null) throw new BadRequestException($"A post with the slug '{request.Slug}' already exists.");

        #region Upload image

        if (request.ImageFile?.Length > 0)
        {
            string uploadImagesFolder = RootPathConfig.UploadPath.PathImagePost;
            if (!Directory.Exists(uploadImagesFolder))
                Directory.CreateDirectory(uploadImagesFolder);

            string extension = Path.GetExtension(request.ImageFile.FileName);

            string imageName = $"{request.Slug}{extension}";
            string filePathImage = Path.Combine(uploadImagesFolder, imageName);

            using (FileStream stream = new(filePathImage, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream, cancellationToken);
            }

            postEntity.ImageUrl = imageName;
        }

        #endregion

        var postAdded =  await _repository.Add(postEntity);
        await _repository.AddCategorieByPost(postAdded, request.CategoryIds);

        return true;
    }
}
