using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Infrastructure.Repositories;

public class PhysicalPersonRepository : EfBaseRepository<PhysicalPerson>, IPhysicalPersonRepository
{
    public PhysicalPersonRepository(PhysicalPersonDbContext dbContext) : base(dbContext)
    {
    }
}