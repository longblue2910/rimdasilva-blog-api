namespace Infrastructure.Common;

public class RepositoryBase<TEntity, TContext> : RepositoryQueryBase<TEntity, TContext>, 
    IRepositoryBaseAsync<TEntity, TContext> where TContext : DbContext
    where TEntity : class
{
    private readonly TContext _dbContext;
    private readonly IUnitOfWork<TContext> _unitOfWork;

    public RepositoryBase(TContext dbContext, IUnitOfWork<TContext> unitOfWork) : base(dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();

    public void Create(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task CreateList(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
        await SaveChangesAsync();
    }

    public async Task CreateListAsync(IEnumerable<TEntity> entities)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await SaveChangesAsync();
    }

    public void DeleteList(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
    }

    public async Task DeleteListAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        await SaveChangesAsync();
    }

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

    public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();

    public void Update(TEntity entity, string id)
    {
        TEntity exist = _dbContext.Set<TEntity>().Find(id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
    }

    public async Task UpdateAsync(TEntity entity, string id)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;

        TEntity exist = _dbContext.Set<TEntity>().Find(id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);

        await SaveChangesAsync();
    }

    public void UpdateList(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
    }

    public async Task UpdateListAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
        await SaveChangesAsync();
    } 

}
