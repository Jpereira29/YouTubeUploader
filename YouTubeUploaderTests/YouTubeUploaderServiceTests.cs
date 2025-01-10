using YouTubeUploader.Services;

namespace YouTubeUploaderTests
{
    public class YouTubeUploaderServiceTests
    {
        [Theory]
        [InlineData("video.mp4")]
        public async Task Upload_ValidStream_ReturnsSuccess(string path)
        {
            // Arrange
            using var stream = new FileStream(path, FileMode.Open);
            // Act
            await YouTubeUploaderService.Upload(stream);
            // Assert
            //Assert.True(result);
        }
    }
}