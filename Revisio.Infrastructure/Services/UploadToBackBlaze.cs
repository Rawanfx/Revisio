using Microsoft.AspNetCore.Http;
using Amazon.S3;
using Revisio.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Amazon.S3.Model;
namespace Revisio.Infrastructure.Services
{
    public class UploadToBackBlaze : Revisio.Application.Common.Interfaces.IUploadToCloud
    {
        private readonly IAmazonS3 _s3Client;
        private readonly B2Setting b2Setting;

        public UploadToBackBlaze(IOptions<B2Setting> options, IAmazonS3 s3Client)
        {
            b2Setting = options.Value;
            this._s3Client = s3Client;
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            string uniqueFileKey = $"{Guid.NewGuid()}_{fileName}";

            var requst = new PutObjectRequest
            {
                BucketName = b2Setting.BucketName,
                Key = uniqueFileKey,
                InputStream = fileStream,
                ContentType = contentType
            };
            await _s3Client.PutObjectAsync(requst, cancellationToken);
            return uniqueFileKey;
        }
        public async Task<string> GenerateUrl(string fileKey)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = b2Setting.BucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddHours(1),
                Verb = HttpVerb.GET
            };
          var url=  await _s3Client.GetPreSignedURLAsync(request);
            return url;
        }

    }
}
