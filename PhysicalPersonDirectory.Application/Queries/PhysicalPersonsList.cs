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

    public Task<PhysicalPersonsQueryResult> Handle(PhysicalPersonsQuery request,
        CancellationToken cancellationToken)
    {
        var physicalPersons = _physicalPersonRepository.Query();

        if (!string.IsNullOrEmpty(request.FirstName))
            physicalPersons = physicalPersons.Where(pp => pp.FirstName.Contains(request.FirstName!));

        if (!string.IsNullOrEmpty(request.LastName))
            physicalPersons = physicalPersons.Where(pp => pp.LastName.Contains(request.LastName!));

        if (!string.IsNullOrEmpty(request.IdentificationNumber))
            physicalPersons = physicalPersons.Where(pp => pp.LastName.Contains(request.IdentificationNumber!));

        if (!string.IsNullOrEmpty(request.IdentificationNumber))
            physicalPersons = physicalPersons.Where(pp => pp.LastName.Contains(request.IdentificationNumber!));

        if (request.CityId.HasValue)
            physicalPersons = physicalPersons.Where(pp => pp.City.Id == request.CityId);

        if (request.StartDateOfBirth.HasValue || request.EndDateOfBirth.HasValue)
            physicalPersons = physicalPersons.Where(pp =>
                (!request.StartDateOfBirth.HasValue || pp.DateOfBirth >= request.StartDateOfBirth.Value)
                && (!request.EndDateOfBirth.HasValue || pp.DateOfBirth <= request.EndDateOfBirth.Value));

        if (!string.IsNullOrEmpty(request.PhoneNumber))
            physicalPersons =
                physicalPersons.Where(pp => pp.PhoneNumbers.Any(pn => pn.Number.Contains(request.PhoneNumber)));

        physicalPersons = physicalPersons
            .Skip(request.PageSize * (request.Page - 1))
            .Take(request.PageSize);

        var imageBaseUrl = _configuration["ImageSettings:ImageBaseUrl"];
        return Task.FromResult(new PhysicalPersonsQueryResult()
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
        });
    }
}

public record PhysicalPersonsQuery(int PageSize, int Page, string? FirstName, string? LastName,
    string? IdentificationNumber, int? CityId, DateTime? StartDateOfBirth,
    DateTime? EndDateOfBirth, string? PhoneNumber) : IRequest<PhysicalPersonsQueryResult>;

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

        public RelatedPhysicalPersonModel[] RelatedPhysicalPersons { get; set; } =
            Array.Empty<RelatedPhysicalPersonModel>();
    }
}