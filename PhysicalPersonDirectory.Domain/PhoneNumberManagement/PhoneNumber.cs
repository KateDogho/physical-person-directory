using System.ComponentModel.DataAnnotations;
using PhysicalPersonDirectory.Domain.Shared;

namespace PhysicalPersonDirectory.Domain.PhoneNumberManagement;

public class PhoneNumber : BaseEntity
{
    public PhoneType Type { get; set; }

    [MinLength(4)]
    [MaxLength(50)]
    public string Number { get; set; } = string.Empty;
}