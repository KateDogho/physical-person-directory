using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO.Abstractions;
using PhysicalPersonDirectory.Application.Services.Abstract;

namespace PhysicalPersonDirectory.Application.Services.Concrete;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;
    private readonly IFileInfoFactory _fileInfoFactory;
    private readonly IFileStreamService _fileStreamService;

    public ImageService(IConfiguration configuration, IFileInfoFactory fileInfoFactory,
        IFileStreamService fileStreamService)
    {
        _configuration = configuration;
        _fileInfoFactory = fileInfoFactory;
        _fileStreamService = fileStreamService;
    }

    public async Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(image.FileName))
            throw new InvalidEnumArgumentException(Resources.Resources.ImageCannotBeUploadedException);

        var path = GetImagePath(image.FileName);

        await using var stream = _fileStreamService.CreateFileStream(path, FileMode.Create);

        await _fileStreamService.CopyImageToStreamAsync(image, stream, cancellationToken);

        return image.FileName;
    }

    public void DeleteImage(string fileName)
    {
        var path = GetImagePath(fileName);
        var fileInfo = _fileInfoFactory.New(path);
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
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