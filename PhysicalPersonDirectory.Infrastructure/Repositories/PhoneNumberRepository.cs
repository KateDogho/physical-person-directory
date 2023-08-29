using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.PhoneNumberManagement;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Infrastructure.Repositories;

public class PhoneNumberRepository : EfBaseRepository<PhoneNumber>, IPhoneNumberRepository
{
    public PhoneNumberRepository(PhysicalPersonDbContext dbContext) : base(dbContext)
    {
    }
}