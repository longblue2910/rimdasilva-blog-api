namespace Post.Domain;

public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable, IUserTracking
{
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string CreateBy { get; set; }
    public string LastModifiedBy { get; set; }
}
