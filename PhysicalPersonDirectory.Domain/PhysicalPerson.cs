using System.ComponentModel.DataAnnotations;

namespace PhysicalPersonDirectory.Domain;

public class PhysicalPerson : BaseEntity
{
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [MinLength(2)]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    [StringLength(11)]
    public string IdentificationNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public virtual City City { get; set; } = new();

    public virtual IEnumerable<PhoneNumber> PhoneNumbers { get; set; } = Enumerable.Empty<PhoneNumber>();

    public string? ImagePath { get; set; } = string.Empty;

    public virtual IEnumerable<RelatedPhysicalPerson> RelatedPhysicalPersons { get; set; } =
        Enumerable.Empty<RelatedPhysicalPerson>();
}