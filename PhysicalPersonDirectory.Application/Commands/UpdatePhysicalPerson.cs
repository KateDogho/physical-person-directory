using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using PhysicalPersonDirectory.Application.Models;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    UpdatePhysicalPersonCommandHandler : IRequestHandler<UpdatePhysicalPersonCommand, UpdatePhysicalPersonCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePhysicalPersonCommandHandler(
        IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork,
        ICityRepository cityRepository)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
        _cityRepository = cityRepository;
    }

    public async Task<UpdatePhysicalPersonCommandResult> Handle(UpdatePhysicalPersonCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPerson = _physicalPersonRepository.OfId(request.Id);
        
        if (physicalPerson is null)
            throw new ArgumentException(Resources.Resources.PhysicalPersonNotFoundException);
        
        var city = _cityRepository.OfId(request.CityId);

        if (city is null)
            throw new ArgumentException(Resources.Resources.CityNotFoundException);

        physicalPerson.FirstName = request.FirstName;
        physicalPerson.LastName = request.LastName;
        physicalPerson.Gender = request.Gender;
        physicalPerson.IdentificationNumber = request.IdentificationNumber;
        physicalPerson.DateOfBirth = request.DateOfBirth;
        physicalPerson.City = city;
        physicalPerson.PhoneNumbers = request.PhoneNumbers.Select(pn => new PhoneNumber
        {
            Type = pn.Type,
            Number = pn.Number
        }).ToList();

        _physicalPersonRepository.Insert(physicalPerson);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new UpdatePhysicalPersonCommandResult(physicalPerson.Id);
    }
}

public record UpdatePhysicalPersonCommand : IRequest<UpdatePhysicalPersonCommandResult>
{
    [NotMapped]
    public int Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public PhoneNumberModel[] PhoneNumbers { get; set; } = Array.Empty<PhoneNumberModel>();

    public int CityId { get; set; }

    public string IdentificationNumber { get; set; } = string.Empty;
}

public record UpdatePhysicalPersonCommandResult(int Id);