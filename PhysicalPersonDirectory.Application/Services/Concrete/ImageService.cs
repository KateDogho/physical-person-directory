using System.ComponentModel;
using System.IO.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PhysicalPersonDirectory.Application.Services.Abstract;

namespace PhysicalPersonDirectory.Application.Services.Concrete;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;
    private readonly IFileSystem _fileInfoFactory;
    private readonly IFileStreamService _fileStreamService;

    public ImageService(IConfiguration configuration, IFileSystem fileInfoFactory,
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
        var fileInfo = _fileInfoFactory.FileInfo.New(path);

        if (fileInfo.Exists)
            fileInfo.Delete();
        else
            throw new ArgumentException(Resources.Resources.ImageDoesnExistException);
    }

    public string GetImageUrl(string fileName)
    {
        var imageBaseUrl = _configuration["ImageSettings:ImageBaseUrl"];

        if (string.IsNullOrEmpty(imageBaseUrl))
            throw new InvalidOperationException(Resources.Resources.ImageBaseUrlIsntConfiguredException);

        return Path.Combine(imageBaseUrl, fileName);
    }

    private static string GetImagePath(string fileName)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", fileName);
    }
}