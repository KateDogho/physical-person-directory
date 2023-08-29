using MediatR;
using PhysicalPersonDirectory.Application.Services.Abstract;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class
    DeletePhysicalPersonCommandHandler : IRequestHandler<DeletePhysicalPersonCommand, DeletePhysicalPersonCommandResult>
{
    private readonly IImageService _imageService;
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IRelatedPhysicalPersonRepository _relatedPhysicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePhysicalPersonCommandHandler(
        IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork,
        IRelatedPhysicalPersonRepository relatedPhysicalPersonRepository,
        IImageService imageService)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
        _relatedPhysicalPersonRepository = relatedPhysicalPersonRepository;
        _imageService = imageService;
    }

    public async Task<DeletePhysicalPersonCommandResult> Handle(DeletePhysicalPersonCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPerson = _physicalPersonRepository.OfId(request.Id);

        if (physicalPerson is null)
            throw new ArgumentException(Resources.Resources.PhysicalPersonNotFoundException);

        var relatedPhysicalPersons =
            _relatedPhysicalPersonRepository.Query(rpp => rpp.TargetPersonId == physicalPerson.Id);

        if (!string.IsNullOrEmpty(physicalPerson.ImagePath)) _imageService.DeleteImage(physicalPerson.ImagePath);

        _physicalPersonRepository.Delete(physicalPerson);
        _relatedPhysicalPersonRepository.Delete(relatedPhysicalPersons);

        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeletePhysicalPersonCommandResult(physicalPerson.Id);
    }
}

public record DeletePhysicalPersonCommand(int Id) : IRequest<DeletePhysicalPersonCommandResult>;

public record DeletePhysicalPersonCommandResult(int Id);