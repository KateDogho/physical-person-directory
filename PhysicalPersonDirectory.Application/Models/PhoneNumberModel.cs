using PhysicalPersonDirectory.Domain;

namespace PhysicalPersonDirectory.Application.Models;

public record PhoneNumberModel(PhoneType Type, string Number);