using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.PhoneNumberManagement;

namespace PhysicalPersonDirectory.Application.Models;

public record PhoneNumberModel(PhoneType Type, string Number);