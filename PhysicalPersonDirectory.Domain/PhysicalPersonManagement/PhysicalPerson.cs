using System.ComponentModel.DataAnnotations;
using PhysicalPersonDirectory.Domain.CityManagement;
using PhysicalPersonDirectory.Domain.PhoneNumberManagement;
using PhysicalPersonDirectory.Domain.Shared;

namespace PhysicalPersonDirectory.Domain.PhysicalPersonManagement;

public class PhysicalPerson : BaseEntity
{
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    [Required] [StringLength(11)] public string IdentificationNumber { get; set; } = string.Empty;

    [Required] public DateTime DateOfBirth { get; set; }

    public virtual City City { get; set; } = new();

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

    public string? ImagePath { get; set; } = string.Empty;
}