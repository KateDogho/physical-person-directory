using MediatR;
using Microsoft.Extensions.Configuration;
using PhysicalPersonDirectory.Application.Models;
using PhysicalPersonDirectory.Application.Services.Abstract;
using PhysicalPersonDirectory.Domain.PhysicalPersonManagement;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Application.Queries;

public class PhysicalPersonsQueryHandler : IRequestHandler<PhysicalPersonsQuery, PhysicalPersonsQueryResult>
{
    private readonly IConfiguration _configuration;
    private readonly IImageService _imageService;
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IRelatedPhysicalPersonRepository _relatedPhysicalPersonRepository;

    public PhysicalPersonsQueryHandler(IPhysicalPersonRepository physicalPersonRepository, IConfiguration configuration,
        IRelatedPhysicalPersonRepository relatedPhysicalPersonRepository, IImageService imageService)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _configuration = configuration;
        _relatedPhysicalPersonRepository = relatedPhysicalPersonRepository;
        _imageService = imageService;
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

        var pageSize = request.PageSize ?? 25;
        var page = request.Page ?? 1;

        var physicalPersonsResult = physicalPersons
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToList();

        var imageBaseUrl = _configuration["ImageSettings:ImageBaseUrl"];

        var physicalPersonIds = physicalPersonsResult.Select(pp => pp.Id).ToArray();

        var relatedPhysicalPersons = _relatedPhysicalPersonRepository
            .Query(rpp => physicalPersonIds.Contains(rpp.TargetPersonId))
            .Select(rrp =>
                new
                {
                    rrp.TargetPersonId,
                    rrp.RelatedPerson.FirstName,
                    rrp.RelatedPerson.LastName,
                    Type = rrp.RelationType
                }).ToArray();

        return Task.FromResult(new PhysicalPersonsQueryResult
        {
            PhysicalPersons = physicalPersonsResult.Select(pp => new PhysicalPersonsQueryResult.PhysicalPerson
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
                    ? _imageService.GetImageUrl(pp.ImagePath)
                    : null,
                RelatedPhysicalPersons = relatedPhysicalPersons.Where(rpp => rpp.TargetPersonId == pp.Id)
                    .Select(rpp =>
                        new RelatedPhysicalPersonModel
                        {
                            FirstName = rpp.FirstName,
                            LastName = rpp.LastName,
                            Type = rpp.Type
                        }).ToArray()
            }).ToArray()
        });
    }
}

public record PhysicalPersonsQuery(int? PageSize, int? Page, string? FirstName, string? LastName,
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