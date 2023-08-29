using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MediatR;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    DeleteRelatedPersonCommandHandler : IRequestHandler<DeleteRelatedPersonCommand, DeleteRelatedPersonCommandResult>
{
    private readonly IRelatedPhysicalPersonRepository _relatedPhysicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRelatedPersonCommandHandler(IUnitOfWork unitOfWork,
        IRelatedPhysicalPersonRepository relatedPhysicalPersonRepository)
    {
        _unitOfWork = unitOfWork;
        _relatedPhysicalPersonRepository = relatedPhysicalPersonRepository;
    }

    public async Task<DeleteRelatedPersonCommandResult> Handle(DeleteRelatedPersonCommand request,
        CancellationToken cancellationToken)
    {
        var relation = _relatedPhysicalPersonRepository.Query(pp =>
                pp.TargetPersonId == request.Id && pp.RelatedPersonId == request.RelatedPersonId)
            .FirstOrDefault();

        if (relation is null)
            throw new ArgumentException(Resources.Resources.RelationNotFoundException);

        _relatedPhysicalPersonRepository.Delete(relation);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteRelatedPersonCommandResult(relation.TargetPersonId, relation.RelatedPersonId);
    }
}

public record DeleteRelatedPersonCommand : IRequest<DeleteRelatedPersonCommandResult>
{
    [NotMapped] [JsonIgnore] public int Id { get; set; }

    public int RelatedPersonId { get; set; }
}

public record DeleteRelatedPersonCommandResult(int TargetPersonId, int RelatedPersonId);