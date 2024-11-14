using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.CommentAggregate;

public class Comment : MongoEntity
{

    public Comment()
    {
        // Đặt CreatedDate và LastModifiedDate theo thời gian hiện tại
        CreatedDate = DateTime.UtcNow;
    }
    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    // Lưu trữ PostId thay vì sử dụng ForeignKey
    [BsonElement("postId")]
    public string PostId { get; set; }

    // Nếu cần có thông tin về Post, bạn có thể sử dụng PostId để tham chiếu và thực hiện truy vấn thêm
    // Mongoose hỗ trợ liên kết qua các ObjectId, nhưng trong MongoDB sẽ cần thực hiện truy vấn riêng biệt
    [BsonIgnore] // Đánh dấu là không lưu trong MongoDB, chỉ để dùng khi truy vấn
    public virtual PostAggregate.Post Post { get; set; }
}