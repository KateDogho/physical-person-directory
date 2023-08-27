using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Infrastructure.Repositories;

public class EfBaseRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class
    {
        private readonly PhysicalPersonDbContext _dbContext;

        protected EfBaseRepository(PhysicalPersonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Delete(TAggregateRoot aggregateRoot)
        {
            _dbContext.Set<TAggregateRoot>().Remove(aggregateRoot);
        }

        public TAggregateRoot? OfId(Guid id)
        {
            return _dbContext.Set<TAggregateRoot>().Find(id);
        }

        public IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default)
        {
            return expression == null ? _dbContext.Set<TAggregateRoot>().AsQueryable() : _dbContext.Set<TAggregateRoot>().Where(expression);
        }

        public void Insert(TAggregateRoot aggregateRoot)
        {
            _dbContext.Set<TAggregateRoot>().AddAsync(aggregateRoot);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            _dbContext.Entry(aggregateRoot).State = EntityState.Modified;
        }
    }