using AutoMapper;
using Contracts.Configurations;
using MediatR;
using Post.Domain.AggregatesModel.PostAggregate;
using Post.Domain.Dtos.Post;

namespace Post.Api.Applications.Commands.Post;

public class CreatePostCommandHandler(IPostRepository repository, IMapper mapper) : IRequestHandler<CreatePostCommand, bool>
{
    private readonly IPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var postEntity = _mapper.Map<CreateOrUpdatePostDto>(request);

        #region Upload image

        string uploadImagesFolder = RootPathConfig.UploadPath.PathImagePost;
        if (!Directory.Exists(uploadImagesFolder))
            Directory.CreateDirectory(uploadImagesFolder);

        string extension = Path.GetExtension(request.ImageFile.FileName);

        string imageName = $"{request.Slug}.{extension}";
        string filePathImage = Path.Combine(uploadImagesFolder, imageName);

        using (FileStream stream = new(filePathImage, FileMode.Create))
        {
            await request.ImageFile.CopyToAsync(stream, cancellationToken);
        }

        #endregion

        postEntity.ImageUrl = imageName;
        await _repository.Add(postEntity);

        return true;
    }
}
