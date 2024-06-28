namespace Infrastructure.Common;

public class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context = context;

    public async Task<int> CommitAsync()
        => await _context.SaveChangesAsync();


    public void Dispose()
    {
        _context.Dispose();
    }
}
