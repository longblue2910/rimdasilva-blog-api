using AutoMapper;
using Contracts.Configurations;
using MediatR;
using MongoDB.Driver;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.Exceptions;
using Post.Infrastructure;

namespace Post.Api.Applications.Commands.Categories;

public class CreateCategoryCommandHandler(MongoDbContext context, IMapper mapper) : IRequestHandler<CreateCategoryCommand, bool>
{
    private readonly IMongoCollection<Category> _categories = context.Categories;
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra xem Category đã tồn tại chưa
        var exist = await _categories.Find(x => x.Slug == request.Slug).FirstOrDefaultAsync(cancellationToken);
        if (exist != null) throw new PostDomainException("Category đã tồn tại.");


        // Map từ request sang entity
        var categoryEntity = _mapper.Map<Category>(request);

        #region Upload image

        // Kiểm tra và upload hình ảnh nếu có file
        if (request?.File?.Length > 0)
        {
            string uploadImagesFolder = RootPathConfig.UploadPath.PathImagePost;
            
            // Kiểm tra và tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadImagesFolder))
            {
                Directory.CreateDirectory(uploadImagesFolder);
            }

            string extension = Path.GetExtension(request.File.FileName);
            string imageName = $"{request.Slug}{extension}";
            string filePathImage = Path.Combine(uploadImagesFolder, imageName);

            // Lưu file vào hệ thống
            using (var stream = new FileStream(filePathImage, FileMode.Create))
            {
                await request.File.CopyToAsync(stream, cancellationToken);
            }

            categoryEntity.ImgUrl = imageName;
        }

        #endregion

        // Thêm Category mới vào MongoDB
        await _categories.InsertOneAsync(categoryEntity, cancellationToken: cancellationToken);
        return true;
    }
}
