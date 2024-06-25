namespace Post.Domain;

public interface IUserTracking
{
    string CreateBy { get; set; }
    string LastModifiedBy { get; set; }
}
