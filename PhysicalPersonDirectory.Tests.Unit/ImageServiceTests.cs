using System.IO.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using PhysicalPersonDirectory.Application.Services.Abstract;
using PhysicalPersonDirectory.Application.Services.Concrete;

namespace PhysicalPersonDirectory.Tests.Unit;

[TestFixture]
public class ImageServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _configurationMock = new Mock<IConfiguration>();
        _fileSystemMock = new Mock<IFileSystem>();
        _fileStreamServiceMock = new Mock<IFileStreamService>();
        _imageService = new ImageService(_configurationMock.Object, _fileSystemMock.Object,
            _fileStreamServiceMock.Object);
    }

    private ImageService _imageService;
    private Mock<IConfiguration> _configurationMock;
    private Mock<IFileSystem> _fileSystemMock;
    private Mock<IFileStreamService> _fileStreamServiceMock;

    [Test]
    public async Task SaveImage_ValidImage_Success()
    {
        // Arrange
        const string fileName = "test.jpg";
        var cancellationToken = CancellationToken.None;
        var imageMock = new Mock<IFormFile>();
        imageMock.Setup(f => f.FileName).Returns(fileName);

        // Act
        var result = await _imageService.SaveImage(imageMock.Object, cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(fileName));
    }

    [Test]
    public void DeleteImage_ExistingImage_Success()
    {
        // Arrange
        var fileName = "test.jpg";
        var fileInfoMock = new Mock<IFileInfo>();
        fileInfoMock.Setup(f => f.Exists).Returns(true);
        fileInfoMock.Setup(f => f.Delete());

        _fileSystemMock.Setup(fs => fs.FileInfo.New(It.IsAny<string>())).Returns(fileInfoMock.Object);

        // Act & Assert
        Assert.DoesNotThrow(() => _imageService.DeleteImage(fileName));
        fileInfoMock.Verify(f => f.Delete(), Times.Once);
    }

    [Test]
    public void DeleteImage_NonExistingImage_ThrowsException()
    {
        // Arrange
        var fileName = "nonexistent.jpg";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/", fileName);
        var fileInfoMock = new Mock<IFileInfo>();
        fileInfoMock.Setup(f => f.Exists).Returns(false);

        _configurationMock.Setup(cfg => cfg["ImageSettings:ImageBaseUrl"]).Returns("TestImagePath");
        _fileSystemMock.Setup(fs => fs.FileInfo.New(filePath)).Returns(fileInfoMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _imageService.DeleteImage(fileName));
        fileInfoMock.Verify(f => f.Delete(), Times.Never);
    }

    [Test]
    public void GetImageUrl_ConfigurationNotSet_ExceptionThrown()
    {
        // Arrange
        _configurationMock.Setup(cfg => cfg["ImageSettings:ImageBaseUrl"]).Returns(string.Empty);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _imageService.GetImageUrl("test.jpg"));
    }
}