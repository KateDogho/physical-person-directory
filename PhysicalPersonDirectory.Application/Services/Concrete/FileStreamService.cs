using Microsoft.AspNetCore.Http;
using PhysicalPersonDirectory.Application.Services.Abstract;

namespace PhysicalPersonDirectory.Application.Services.Concrete;

public class FileStreamService : IFileStreamService
{
    public FileStream CreateFileStream(string path, FileMode mode)
    {
        return new FileStream(path, mode);
    }

    public async Task CopyImageToStreamAsync(IFormFile image, Stream stream, CancellationToken cancellationToken)
    {
        await image.CopyToAsync(stream, cancellationToken);
    }
}