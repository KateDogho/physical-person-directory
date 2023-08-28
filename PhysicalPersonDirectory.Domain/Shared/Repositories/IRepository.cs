using System.Linq.Expressions;

namespace PhysicalPersonDirectory.Domain.Shared.Repositories;

public interface IRepository<TAggregateRoot> where TAggregateRoot : class
{
    TAggregateRoot? OfId(int id);

    void Delete(TAggregateRoot aggregateRoot);
    
    void Delete(IEnumerable<TAggregateRoot> aggregateRoot);
    
    void Insert(TAggregateRoot aggregateRoot);

    void Insert(IEnumerable<TAggregateRoot> aggregateRoot);

    void Update(TAggregateRoot aggregateRoot);

    IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default);
}