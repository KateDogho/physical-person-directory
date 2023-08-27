using PhysicalPersonDirectory.Domain;

namespace PhysicalPersonDirectory.Application.Models;

public record RelatedPhysicalPersonModel
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public RelationType Type { get; set; }
}