using Microsoft.AspNetCore.Http;

namespace Revisio.Application.Common.Interfaces
{
    public interface IUploadToCloud
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task<string> GenerateUrl(string fileKey);
    }
}
