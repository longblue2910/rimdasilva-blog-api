namespace Post.Domain;

public interface IEntityBase<T>
{
    T Id { get; set; }
}
