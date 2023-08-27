using MediatR;
using Microsoft.Extensions.Configuration;
using PhysicalPersonDirectory.Application.Models;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Application.Queries;

public class PhysicalPersonsQueryHandler : IRequestHandler<PhysicalPersonsQuery, PhysicalPersonsQueryResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IConfiguration _configuration;

    public PhysicalPersonsQueryHandler(IPhysicalPersonRepository physicalPersonRepository, IConfiguration configuration)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _configuration = configuration;
    }

    public async Task<PhysicalPersonsQueryResult> Handle(PhysicalPersonsQuery request,
        CancellationToken cancellationToken)
    {
        var physicalPersons = _physicalPersonRepository.Query();
        var imageBaseUrl = _configuration["ImageSettings:ImageBaseUrl"];
        return new PhysicalPersonsQueryResult()
        {
            PhysicalPersons = physicalPersons.Select(pp => new PhysicalPersonsQueryResult.PhysicalPerson
            {
                Id = pp.Id,
                FirstName = pp.FirstName,
                LastName = pp.LastName,
                Gender = pp.Gender,
                IdentificationNumber = pp.IdentificationNumber,
                DateOfBirth = pp.DateOfBirth,
                City = pp.City.Name,
                PhoneNumbers = pp.PhoneNumbers.Select(pn => new PhoneNumberModel(
                    pn.Type,
                    pn.Number
                )).ToArray(),
                ImagePath = !string.IsNullOrEmpty(pp.ImagePath)
                    ? Path.Combine(imageBaseUrl, pp.ImagePath)
                    : null,
                RelatedPhysicalPersons = pp.RelatedPhysicalPersons.Select(rrp =>
                    new RelatedPhysicalPersonModel
                    {
                        FirstName = rrp.RelatedPerson.FirstName,
                        LastName = rrp.RelatedPerson.LastName,
                        Type = rrp.RelationType
                    }).ToArray()
            }).ToArray()
        };
    }
}

public record PhysicalPersonsQueryResult
{
    public PhysicalPerson[] PhysicalPersons { get; set; } = Array.Empty<PhysicalPerson>();

    public record PhysicalPerson
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

        public RelatedPhysicalPersonModel[] RelatedPhysicalPersons { get; set; } = Array.Empty<RelatedPhysicalPersonModel>();
    }
}

public record PhysicalPersonsQuery : IRequest<PhysicalPersonsQueryResult>
{
}