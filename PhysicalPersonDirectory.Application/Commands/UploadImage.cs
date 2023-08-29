using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using PhysicalPersonDirectory.Application.Services.Abstract;
using PhysicalPersonDirectory.Domain.Repositories;
using PhysicalPersonDirectory.Domain.Shared.Repositories;

namespace PhysicalPersonDirectory.Application.Commands;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, UploadImageCommandResult>
{
    private readonly IPhysicalPersonRepository _physicalPersonRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public UploadImageCommandHandler(IPhysicalPersonRepository physicalPersonRepository,
        IUnitOfWork unitOfWork, 
        IImageService imageService)
    {
        _physicalPersonRepository = physicalPersonRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task<UploadImageCommandResult> Handle(UploadImageCommand request,
        CancellationToken cancellationToken)
    {
        var physicalPerson = _physicalPersonRepository.OfId(request.Id);

        if (physicalPerson is null)
            throw new InvalidEnumArgumentException(Resources.Resources.PhysicalPersonNotFoundException);

        if (string.IsNullOrEmpty(request.Image.FileName))
            throw new InvalidEnumArgumentException(Resources.Resources.ImageCannotBeUploadedException);

        if (!string.IsNullOrEmpty(physicalPerson.ImagePath))
        {
            _imageService.DeleteImage(physicalPerson.ImagePath);
        }

        var fileName = await _imageService.SaveImage(request.Image, cancellationToken);
        
        physicalPerson.ImagePath = fileName;

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