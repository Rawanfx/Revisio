using Microsoft.AspNetCore.Http;

namespace Revisio.Application.Common.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string mailTo, string? subject, List<IFormFile> attachments, string body);
        Task SendConfirmationEmail(string fileName, string replaceWord, string url, string Email);
    }
}
