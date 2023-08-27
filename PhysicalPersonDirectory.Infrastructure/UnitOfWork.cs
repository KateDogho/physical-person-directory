namespace PhysicalPersonDirectory.Infrastructure;


public class UnitOfWork
{
    private readonly PhysicalPersonDbContext _db;

    public UnitOfWork(PhysicalPersonDbContext db)
    {
        _db = db;
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

    public async Task SaveAsync(bool useTransaction = true)
    {
        if (useTransaction)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        else
        {
            await _db.SaveChangesAsync();
        }
    }
}