using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Reflection;

namespace YouTubeUploader.Services
{
    public class YouTubeUploaderService
    {
        public static async Task Upload(FileStream fileStream)
        {
            UserCredential credential;

            using var secretsStream = new FileStream("Properties/client_secrets.json", FileMode.Open, FileAccess.Read);
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(secretsStream).Secrets,
                [YouTubeService.Scope.YoutubeUpload],
                "Cliente Web 1",
                CancellationToken.None
            );

            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var video = new Video
            {
                Snippet = new VideoSnippet
                {
                    Title = "Default Video Title",
                    Description = "Default Video Description",
                    Tags = ["tag1", "tag2"],
                    CategoryId = "22" // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                },
                Status = new VideoStatus
                {
                    PrivacyStatus = "private" // or "unlisted" or "public"
                }
            };

            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }
            var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
            var uploadResult = await videosInsertRequest.UploadAsync();
        }
    }
}
