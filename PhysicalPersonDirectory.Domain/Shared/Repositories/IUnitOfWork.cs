namespace PhysicalPersonDirectory.Domain.Shared.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> CommitAsync(CancellationToken cancellationToken);
}