using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace PhysicalPersonDirectory.Application.Commands;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileCommandResult>
{
    public async Task<UploadFileCommandResult> Handle(UploadFileCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Image.FileName))
        {
            throw new InvalidEnumArgumentException("Image can't be uploaded");
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", request.Image.FileName);

        await using var stream = new FileStream(path, FileMode.Create);

        await request.Image.CopyToAsync(stream, cancellationToken);
        stream.Close();

        return new UploadFileCommandResult(request.Image.FileName);
    }
}

public record UploadFileCommand(IFormFile Image) : IRequest<UploadFileCommandResult>
{
}

public record UploadFileCommandResult(string FileName);