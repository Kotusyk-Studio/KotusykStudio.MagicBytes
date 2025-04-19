using Xunit;

namespace KotusykStudio.MagicBytes.UnitTests;
public class ByteSnifferTests
{
    [Fact]
    public void GetFileExtension_ValidJpg_ReturnsJpg()
    {
        // Arrange
        var fileBytes = new byte[] { 255, 216, 255, 224 }; // JPG magic number
        // Act
        var result = ByteSniffer.GetFileExtension(fileBytes);
        // Assert
        Assert.Equal(".jpg", result);
    }

    [Fact]
    public void GetFileExtension_ValidBmp_ReturnsBmp()
    {
        // Arrange
        var fileBytes = new byte[] { 66, 77 }; // BMP magic number
        // Act
        var result = ByteSniffer.GetFileExtension(fileBytes);
        // Assert
        Assert.Equal(".bmp", result);
    }

    [Fact]
    public void GetFileExtension_UnknownFormat_ReturnsUnknown()
    {
        // Arrange
        var fileBytes = new byte[] { 0x00, 0x00, 0x00 }; // Unknown format
        // Act
        var result = ByteSniffer.GetFileExtension(fileBytes);
        // Assert
        Assert.Equal(".unknown", result);
    }
}
