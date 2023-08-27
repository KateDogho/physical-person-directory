using System.ComponentModel.DataAnnotations;

namespace PhysicalPersonDirectory.Domain;

public class PhoneNumber : BaseEntity
{
    public PhoneType Type { get; set; }

    [MinLength(4)]
    [MaxLength(50)]
    public string Number { get; set; } = string.Empty;
}