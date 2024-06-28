using AutoMapper;
using Contracts.Configurations;
using MediatR;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Commands.Post;

public class UpdatePostCommandHandler(IPostRepository repository, IMapper mapper) : IRequestHandler<UpdatePostCommand, bool>
{
    private readonly IPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var postEntity = await _repository.FindByIdAsync(request.Id.ToString());
        postEntity = _mapper.Map(request, postEntity);

        #region Upload image

        if (request.ImageFile.Length > 0)
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

        var postUpdated = await _repository.Update(postEntity);
        await _repository.AddCategorieByPost(postUpdated, request.CategoryIds);
        

        return true;
    }
}
