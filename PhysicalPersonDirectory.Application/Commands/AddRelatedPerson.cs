using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MediatR;
using PhysicalPersonDirectory.Domain.PhysicalPersonManagement;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    AddRelatedPersonCommandHandler : IRequestHandler<AddRelatedPersonCommand, AddRelatedPersonCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IRelatedPhysicalPersonRepository _relatedPhysicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddRelatedPersonCommandHandler(
        IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork, IRelatedPhysicalPersonRepository relatedPhysicalPersonRepository)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
        _relatedPhysicalPersonRepository = relatedPhysicalPersonRepository;
    }

    public async Task<AddRelatedPersonCommandResult> Handle(AddRelatedPersonCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPersons = _physicalPersonRepository.Query(pp => pp.Id == request.Id
                                                                    || pp.Id == request.RelatedPersonId);

        if (physicalPersons.All(pp => pp.Id != request.Id))
            throw new ArgumentException(Resources.Resources.PhysicalPersonNotFoundException);

        if (!physicalPersons.All(pp => pp.Id != request.RelatedPersonId))
            throw new ArgumentException(Resources.Resources.RelatedPersonNotFoundException);

        var target = physicalPersons.First(pp => pp.Id == request.Id);
        var related = physicalPersons.First(pp => pp.Id == request.RelatedPersonId);

        var relatedPhysicalPersons = _relatedPhysicalPersonRepository.Query(rpp => rpp.TargetPersonId == request.Id);

        if (relatedPhysicalPersons.Any(rpp => rpp.RelatedPersonId == request.RelatedPersonId))
            throw new ArgumentException(Resources.Resources.RelatedPersonAlreadyExistsException);

        var relation = new RelatedPhysicalPerson
        {
            RelationType = request.Type,
            TargetPerson = target,
            RelatedPerson = related
        };

        _relatedPhysicalPersonRepository.Insert(relation);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new AddRelatedPersonCommandResult(target.Id, related.Id);
    }
}

public record AddRelatedPersonCommand : IRequest<AddRelatedPersonCommandResult>
{
    [NotMapped] [JsonIgnore] public int Id { get; set; }

    public RelationType Type { get; set; }

    public int RelatedPersonId { get; set; }
}

public record AddRelatedPersonCommandResult(int TargetPersonId, int RelatedPersonId);