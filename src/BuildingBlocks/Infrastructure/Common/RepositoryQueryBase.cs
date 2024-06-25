namespace Infrastructure.Common;

public class RepositoryQueryBase<TEntity, TContext> : IRepositoryQueryBase<TEntity, TContext>
    where TContext : DbContext
    where TEntity : class
{
    private readonly TContext _dbContext;

    public RepositoryQueryBase(TContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return !trackChanges ? _dbContext.Set<TEntity>().AsNoTracking() : _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false) =>
           !trackChanges ? _dbContext.Set<TEntity>().Where(expression).AsNoTracking()
                         : _dbContext.Set<TEntity>().Where(expression);

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> expression) 
        => await FindByCondition(expression).FirstOrDefaultAsync();

    public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> includeQueryable = _dbContext.Set<TEntity>();
        foreach (var includeExpression in includeProperties)
        {
            includeQueryable = includeQueryable.Include(includeExpression);
        }

        return includeQueryable.FirstOrDefaultAsync(expression);
    }
}
