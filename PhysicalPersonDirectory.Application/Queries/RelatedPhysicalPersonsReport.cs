using MediatR;
using PhysicalPersonDirectory.Domain.PhysicalPersonManagement;
using PhysicalPersonDirectory.Domain.Repositories;

namespace PhysicalPersonDirectory.Application.Queries;

public class
    RelatedPhysicalPersonsReportQueryHandler : IRequestHandler<RelatedPhysicalPersonsReportQuery,
        RelatedPhysicalPersonsReportQueryResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IRelatedPhysicalPersonRepository _relatedPhysicalPersonRepository;

    public RelatedPhysicalPersonsReportQueryHandler(IPhysicalPersonRepository physicalPersonRepository,
        IRelatedPhysicalPersonRepository relatedPhysicalPersonRepository)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _relatedPhysicalPersonRepository = relatedPhysicalPersonRepository;
    }

    public Task<RelatedPhysicalPersonsReportQueryResult> Handle(RelatedPhysicalPersonsReportQuery request,
        CancellationToken cancellationToken)
    {
        var relatedPhysicalPersons = _relatedPhysicalPersonRepository.Query()
            .GroupBy(rpp => rpp.TargetPersonId)
            .Select(pp => new RelatedPhysicalPersonsReportQueryResult.RelatedPerson
            {
                RelatedPersonId = pp.Key,
                PersonTypes = pp.GroupBy(rpp => rpp.RelationType).Select(rpp =>
                    new RelatedPhysicalPersonsReportQueryResult.RelatedPersonType
                    {
                        RelationType = rpp.Key,
                        RelatedPersonsCount = rpp.Count()
                    })
            })
            .ToList();

        return Task.FromResult(new RelatedPhysicalPersonsReportQueryResult(relatedPhysicalPersons));
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