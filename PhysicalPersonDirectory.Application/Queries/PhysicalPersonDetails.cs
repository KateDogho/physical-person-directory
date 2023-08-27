using System.ComponentModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhysicalPersonDirectory.Application.Models;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Application.Queries;

public class
    PhysicalPersonDetailsQueryHandler : IRequestHandler<PhysicalPersonDetailsQuery, PhysicalPersonDetailsQueryResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IConfiguration _configuration;

    public PhysicalPersonDetailsQueryHandler(IPhysicalPersonRepository physicalPersonRepository,
        IConfiguration configuration)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _configuration = configuration;
    }

    public async Task<PhysicalPersonDetailsQueryResult> Handle(PhysicalPersonDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var physicalPerson = _physicalPersonRepository.Query(pp => pp.Id == request.Id)
            .Include(pp => pp.PhoneNumbers)
            .Include(pp => pp.RelatedPhysicalPersons)
            .Include(pp => pp.City)
            .FirstOrDefault();

        if (physicalPerson is null)
            throw new InvalidEnumArgumentException("Physical Person not found");

        var imageBaseUrl = _configuration["ImageSettings:ImageBaseUrl"];

        return new PhysicalPersonDetailsQueryResult()
        {
            Id = physicalPerson.Id,
            FirstName = physicalPerson.FirstName,
            LastName = physicalPerson.LastName,
            Gender = physicalPerson.Gender,
            IdentificationNumber = physicalPerson.IdentificationNumber,
            DateOfBirth = physicalPerson.DateOfBirth,
            City = physicalPerson.City.Name,
            PhoneNumbers = physicalPerson.PhoneNumbers.Select(pn => new PhoneNumberModel(
                pn.Type,
                pn.Number
            )).ToArray(),
            ImagePath = !string.IsNullOrEmpty(physicalPerson.ImagePath)
                ? Path.Combine(imageBaseUrl, physicalPerson.ImagePath)
                : null,
            RelatedPhysicalPersons = physicalPerson.RelatedPhysicalPersons.Select(rrp =>
                new RelatedPhysicalPersonModel
                {
                    FirstName = rrp.RelatedPerson.FirstName,
                    LastName = rrp.RelatedPerson.LastName,
                    Type = rrp.RelationType
                }).ToArray()
        };
    }
}

public record PhysicalPersonDetailsQueryResult
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public string IdentificationNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string City { get; set; } = string.Empty;

    public PhoneNumberModel[] PhoneNumbers { get; set; } = Array.Empty<PhoneNumberModel>();

    public string? ImagePath { get; set; } = string.Empty;

    public RelatedPhysicalPersonModel[] RelatedPhysicalPersons { get; set; } =
        Array.Empty<RelatedPhysicalPersonModel>();
}

public record PhysicalPersonDetailsQuery(int Id) : IRequest<PhysicalPersonDetailsQueryResult>
{
}