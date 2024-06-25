namespace Post.Domain;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
    public bool? IsDelete { get; set; }
}
