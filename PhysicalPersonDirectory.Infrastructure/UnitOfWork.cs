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

    public IRepository<T> Repository<T>() where T : class
    {
        return new EfBaseRepository<T>(_db);
    }

    public async Task SaveAsync(CancellationToken cancellationToken, bool useTransaction = true)
    {
        if (useTransaction)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
    }

    public void Save(bool useTransaction = true)
    {
        if (useTransaction)
        {
            using var transaction = _db.Database.BeginTransaction();

            _db.SaveChanges();

            transaction.Commit();
        }
        else
        {
            _db.SaveChanges();
        }
    }
}