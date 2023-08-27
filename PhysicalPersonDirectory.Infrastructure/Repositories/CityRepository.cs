using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Infrastructure.Repositories;

public class CityRepository : EfBaseRepository<City>, ICityRepository
{
    public CityRepository(PhysicalPersonDbContext dbContext) : base(dbContext)
    {
    }
}