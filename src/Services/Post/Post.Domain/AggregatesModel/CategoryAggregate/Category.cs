using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.CategoryAggregate;


public class Category : MongoEntity
{
    // Constructor không nhận tham số Id, MongoDB sẽ tự động sinh Id khi insert
    public Category()
    {
        // Đặt CreatedDate và LastModifiedDate theo thời gian hiện tại
        CreatedDate = DateTime.UtcNow;
    }

    [BsonElement("slug")]
    public string Slug { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("imgUrl")]
    public string ImgUrl { get; set; }

    [BsonElement("orderIndex")]
    public int OrderIndex { get; set; }

    [BsonElement("tagName")]
    public string TagName { get; set; }

    [BsonElement("posts")]
    public List<PostAggregate.Post> Posts { get; set; } = [];
        [BsonElement("children")]
    public List<Category> Children { get; set; } = [];
}

