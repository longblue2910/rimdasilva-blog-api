using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.UserAggregate;

public class User : MongoEntity
{

    public User()
    {
        // Đặt CreatedDate và LastModifiedDate theo thời gian hiện tại
        CreatedDate = DateTime.UtcNow;
    }
    [BsonElement("userName")]
    public string Username { get; set; }

    [BsonElement("fullName")]
    public string FullName { get; set; }

    [BsonElement("userType")]
    public UserType UserType { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("phone")]
    public string Phone { get; set; }

    [BsonElement("passwordHash")]
    public string Password { get; set; }
}
