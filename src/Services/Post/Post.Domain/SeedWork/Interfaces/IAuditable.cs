namespace Post.Domain;

public interface IAuditable : IDateTracking  
{
    public string CreateBy { get; set; }
    public string LastModifiedBy { get; set; }

}
