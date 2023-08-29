using Microsoft.AspNetCore.Http;

namespace PhysicalPersonDirectory.Application.Services.Abstract;

public interface IImageService
{
    Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken);
    
    void DeleteImage(string fileName);
    
    string GetImageUrl(string fileName);
}