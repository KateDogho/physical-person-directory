using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PhysicalPersonDirectory.Application.Services.Abstract;

namespace PhysicalPersonDirectory.Application.Services.Concrete;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;

    public ImageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(image.FileName))
            throw new InvalidEnumArgumentException(Resources.Resources.ImageCannotBeUploadedException);

        var path = GetImagePath(image.FileName);
        
        await using var stream = new FileStream(path, FileMode.Create);

        await image.CopyToAsync(stream, cancellationToken);
        stream.Close();

        return image.FileName;
    }

    public void DeleteImage(string fileName)
    {
        var path = GetImagePath(fileName);
        var file = new FileInfo(path);
        if (file.Exists)
        {
            file.Delete();
        }
        else
        {
            throw new ArgumentException("Image doesn't exist");
        }
    }

    public string GetImageUrl(string fileName)
    {
        var imageBaseUrl = _configuration["ImageSettings:ImageBaseUrl"];

        if (string.IsNullOrEmpty(imageBaseUrl))
            throw new InvalidOperationException("Image Base Url isn't configured");
        
        return Path.Combine(imageBaseUrl, fileName);
    }
    
    private static string GetImagePath(string fileName) =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", fileName);
}