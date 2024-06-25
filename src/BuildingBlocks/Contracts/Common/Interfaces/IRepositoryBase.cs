namespace Contracts.Common.Interfaces;

public interface IRepositoryQueryBase<TEntity, TContext> where TContext : DbContext
    where TEntity : class
{
    IQueryable<TEntity> FindAll(bool trackChanges = false);
    IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
}

public interface IRepositoryBaseAsync<TEntity, TContext> : IRepositoryQueryBase<TEntity, TContext>
    where TContext : DbContext
    where TEntity : class

{
    void Create(TEntity entity);
    Task CreateAsync(TEntity entity);
    Task CreateList(IEnumerable<TEntity> entities);
    Task CreateListAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity, Guid id);
    Task UpdateAsync(TEntity entity, Guid id);
    void UpdateList(IEnumerable<TEntity> entities);
    Task UpdateListAsync(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    Task DeleteAsync(TEntity entity);
    void DeleteList(IEnumerable<TEntity> entities);
    Task DeleteListAsync(IEnumerable<TEntity> entities);
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();
}
