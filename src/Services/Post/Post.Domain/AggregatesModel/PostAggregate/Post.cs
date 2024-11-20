using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.CommentAggregate;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.PostAggregate
{
    public class Post : MongoEntity
    {
        public Post()
        {
            // Đặt CreatedDate và LastModifiedDate theo thời gian hiện tại
            CreatedDate = DateTime.UtcNow;
        }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("slug")]
        public string Slug { get; set; }

        // Các danh sách liên kết đến Category và Comment
        [BsonElement("categories")]
        public List<Category> Categories { get; set; } = [];

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; } = [];

    }
}
