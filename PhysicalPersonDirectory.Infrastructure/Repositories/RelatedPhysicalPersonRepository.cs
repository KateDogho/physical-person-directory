using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.PhysicalPersonManagement;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Infrastructure.Repositories;

public class RelatedPhysicalPersonRepository : EfBaseRepository<RelatedPhysicalPerson>, IRelatedPhysicalPersonRepository
{
    public RelatedPhysicalPersonRepository(PhysicalPersonDbContext dbContext) : base(dbContext)
    {
    }
}