using MediatR;
using PhysicalPersonDirectory.Domain;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class CreatePhysicalPersonCommandHandler:  IRequestHandler<CreatePhysicalPersonCommand, CreatePhysicalPersonCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePhysicalPersonCommandHandler(IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePhysicalPersonCommandResult> Handle(CreatePhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var physicalPerson = new PhysicalPerson()
        {
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        _physicalPersonRepository.Insert(physicalPerson);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreatePhysicalPersonCommandResult(physicalPerson.Id);
    }
}


public record CreatePhysicalPersonCommand(string FirstName, string LastName) : IRequest<CreatePhysicalPersonCommandResult>
{
}

public record CreatePhysicalPersonCommandResult(int Id)
{
}