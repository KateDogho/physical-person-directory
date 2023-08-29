using MediatR;
using PhysicalPersonDirectory.Application.Models;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    CreatePhysicalPersonCommandHandler : IRequestHandler<CreatePhysicalPersonCommand, CreatePhysicalPersonCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePhysicalPersonCommandHandler(
        IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork,
        ICityRepository cityRepository)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
        _cityRepository = cityRepository;
    }

    public async Task<CreatePhysicalPersonCommandResult> Handle(CreatePhysicalPersonCommand request,
        CancellationToken cancellationToken)
    {
        var city = _cityRepository.OfId(request.CityId);

        if (city is null)
            throw new ArgumentException(Resources.Resources.CityNotFoundException);

        var physicalPerson = new PhysicalPerson
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            IdentificationNumber = request.IdentificationNumber,
            DateOfBirth = request.DateOfBirth,
            City = city,
            PhoneNumbers = request.PhoneNumbers.Select(pn => new PhoneNumber
            {
                Type = pn.Type,
                Number = pn.Number
            }).ToList()
        };

        _physicalPersonRepository.Update(physicalPerson);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreatePhysicalPersonCommandResult(physicalPerson.Id);
    }
}

public record CreatePhysicalPersonCommand : IRequest<CreatePhysicalPersonCommandResult>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public PhoneNumberModel[] PhoneNumbers { get; set; } = Array.Empty<PhoneNumberModel>();

    public int CityId { get; set; }

    public string IdentificationNumber { get; set; } = string.Empty;
}

public record CreatePhysicalPersonCommandResult(int Id);