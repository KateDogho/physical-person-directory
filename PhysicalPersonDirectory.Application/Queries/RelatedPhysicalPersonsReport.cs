using MediatR;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Application.Queries;

public class
    RelatedPhysicalPersonsReportQueryHandler : IRequestHandler<RelatedPhysicalPersonsReportQuery,
        RelatedPhysicalPersonsReportQueryResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;

    public RelatedPhysicalPersonsReportQueryHandler(IPhysicalPersonRepository physicalPersonRepository)
    {
        _physicalPersonRepository = physicalPersonRepository;
    }

    public Task<RelatedPhysicalPersonsReportQueryResult> Handle(RelatedPhysicalPersonsReportQuery request,
        CancellationToken cancellationToken)
    {
        var physicalPersons = _physicalPersonRepository.Query(pp => pp.RelatedPhysicalPersons.Any())
            .Select(pp => new RelatedPhysicalPersonsReportQueryResult.RelatedPerson
            {
                RelatedPersonId = pp.Id,
                PersonTypes = pp.RelatedPhysicalPersons.GroupBy(rpp => rpp.RelationType).Select(rpp =>
                    new RelatedPhysicalPersonsReportQueryResult.RelatedPersonType
                    {
                        RelationType = rpp.Key,
                        RelatedPersonsCount = rpp.Count()
                    })
            })
            .ToList();

        return Task.FromResult(new RelatedPhysicalPersonsReportQueryResult(physicalPersons));
    }
}

public record RelatedPhysicalPersonsReportQueryResult(
    IEnumerable<RelatedPhysicalPersonsReportQueryResult.RelatedPerson> RelatedPersons)
{
    public record RelatedPerson
    {
        public int RelatedPersonId { get; set; }

        public IEnumerable<RelatedPersonType> PersonTypes { get; set; } = Enumerable.Empty<RelatedPersonType>();
    }

    public record RelatedPersonType
    {
        public RelationType RelationType { get; set; }

        public int RelatedPersonsCount { get; set; }
    }
}

public record RelatedPhysicalPersonsReportQuery : IRequest<RelatedPhysicalPersonsReportQueryResult>;