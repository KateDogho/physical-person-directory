using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    DeleteRelatedPersonCommandHandler : IRequestHandler<DeleteRelatedPersonCommand, DeleteRelatedPersonCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRelatedPersonCommandHandler(
        IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteRelatedPersonCommandResult> Handle(DeleteRelatedPersonCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPerson = _physicalPersonRepository.Query(pp => pp.Id == request.Id)
            .Include(pp=>pp.RelatedPhysicalPersons)
            .FirstOrDefault();

        if (physicalPerson is null)
            throw new ArgumentException("Physical person not found");

        var relatedPerson =
            physicalPerson.RelatedPhysicalPersons.FirstOrDefault(pp => pp.RelatedPersonId == request.RelatedPersonId);
        if (relatedPerson is null)
            throw new ArgumentException("Related person not found");

        physicalPerson.RelatedPhysicalPersons.Remove(relatedPerson);

        _physicalPersonRepository.Update(physicalPerson);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteRelatedPersonCommandResult(physicalPerson.Id, relatedPerson.RelatedPersonId);
    }
}

public record DeleteRelatedPersonCommand : IRequest<DeleteRelatedPersonCommandResult>
{
    [NotMapped] public int Id { get; set; }

    public int RelatedPersonId { get; set; }
}

public record DeleteRelatedPersonCommandResult(int TargetPersonId, int RelatedPersonId);