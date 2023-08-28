using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, UploadImageCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadImageCommandHandler(IPhysicalPersonRepository physicalPersonRepository, IUnitOfWork unitOfWork)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UploadImageCommandResult> Handle(UploadImageCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPerson = _physicalPersonRepository.OfId(request.Id);

        if (physicalPerson is null)
            throw new InvalidEnumArgumentException(Resources.PhysicalPersonNotFoundException);

        if (string.IsNullOrEmpty(request.Image.FileName))
            throw new InvalidEnumArgumentException(Resources.ImageCannotBeUploadedException);

        if (!string.IsNullOrEmpty(physicalPerson.ImagePath))
        {
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", physicalPerson.ImagePath);
            var file = new FileInfo(oldPath);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", request.Image.FileName);

        await using var stream = new FileStream(path, FileMode.Create);

        await request.Image.CopyToAsync(stream, cancellationToken);
        stream.Close();

        physicalPerson.ImagePath = request.Image.FileName;

        _physicalPersonRepository.Update(physicalPerson);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new UploadImageCommandResult(request.Image.FileName);
    }
}

public record UploadImageCommand : IRequest<UploadImageCommandResult>
{
    public int Id { get; set; }

    public IFormFile Image { get; set; }
}

public record UploadImageCommandResult(string FileName);