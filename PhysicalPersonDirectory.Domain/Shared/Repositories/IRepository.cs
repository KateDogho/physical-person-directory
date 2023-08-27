using System.Linq.Expressions;

namespace PhysicalPersonDirectory.Domain.Shared.Repositories;

public interface IRepository<TAggregateRoot> where TAggregateRoot : class
{
    TAggregateRoot? OfId(Guid id);

    void Delete(TAggregateRoot aggregateRoot);

    void Insert(TAggregateRoot aggregateRoot);

    void Update(TAggregateRoot aggregateRoot);

    IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default);
}