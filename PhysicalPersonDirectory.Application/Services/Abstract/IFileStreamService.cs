using Microsoft.AspNetCore.Http;

namespace PhysicalPersonDirectory.Application.Services.Abstract;

public interface IFileStreamService
{
    FileStream CreateFileStream(string path, FileMode mode);
    
    Task CopyImageToStreamAsync(IFormFile image, Stream stream, CancellationToken cancellationToken);
}