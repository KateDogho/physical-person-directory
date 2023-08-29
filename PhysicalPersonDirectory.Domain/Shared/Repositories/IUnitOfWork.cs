namespace PhysicalPersonDirectory.Domain.Shared.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;

    Task SaveAsync(CancellationToken cancellationToken, bool useTransaction = true);
}