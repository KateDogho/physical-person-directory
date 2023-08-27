using Microsoft.EntityFrameworkCore;
using PhysicalPersonDirectory.Domain.Shared.Repositories;
using PhysicalPersonDirectory.Infrastructure.Repositories;

namespace PhysicalPersonDirectory.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PhysicalPersonDbContext _db;
    
    public UnitOfWork(PhysicalPersonDbContext db)
    {
        _db = db;
    }
    
    public void Commit()
    {
        _db.SaveChanges();
    }
    public void Rollback()
    {
        foreach (var entry in _db.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
    public IRepository<T> Repository<T>() where T : class
    {
        return new EfBaseRepository<T>(_db);
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }

    private bool _disposed = false;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}