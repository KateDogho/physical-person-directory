using PhysicalPersonDirectory.Domain.Shared;

namespace PhysicalPersonDirectory.Domain.CityManagement;

public class City : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}