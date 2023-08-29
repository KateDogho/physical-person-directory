using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;
using RelatedPhysicalPerson = PhysicalPersonDirectory.Domain.RelatedPhysicalPerson;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    AddRelatedPersonCommandHandler : IRequestHandler<AddRelatedPersonCommand, AddRelatedPersonCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddRelatedPersonCommandHandler(
        IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddRelatedPersonCommandResult> Handle(AddRelatedPersonCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPersons = _physicalPersonRepository.Query(pp => pp.Id == request.Id
                                                                    || pp.Id == request.RelatedPersonId);

        if (!physicalPersons.Any(pp => pp.Id != request.Id))
            throw new ArgumentException(Resources.Resources.PhysicalPersonNotFoundException);

        if (!physicalPersons.Any(pp => pp.Id != request.RelatedPersonId))
            throw new ArgumentException(Resources.Resources.RelatedPersonNotFoundException);

        var target = physicalPersons.First(pp => pp.Id == request.Id);
        if (target.RelatedPhysicalPersons.All(rpp => rpp.RelatedPersonId != request.RelatedPersonId))
            throw new ArgumentException(Resources.Resources.RelatedPersonAlreadyExistsException);

        var relatedPerson = physicalPersons.First(pp => pp.Id == request.RelatedPersonId);
        target.RelatedPhysicalPersons.Add(new RelatedPhysicalPerson
        {
            RelationType = request.Type,
            RelatedPerson = relatedPerson
        });

        _physicalPersonRepository.Update(target);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AddRelatedPersonCommandResult(target.Id, relatedPerson.Id);
    }
}

public record AddRelatedPersonCommand : IRequest<AddRelatedPersonCommandResult>
{
    [NotMapped] public int Id { get; set; }

    public RelationType Type { get; set; }

    public int RelatedPersonId { get; set; }
}

public record AddRelatedPersonCommandResult(int TargetPersonId, int RelatedPersonId);