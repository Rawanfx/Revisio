using Microsoft.AspNetCore.Http;

namespace Revisio.Application.Common.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string mailTo, string? subject, List<IFormFile> attachments, string body);
        Task SendConfirmationEmail(  string Email,string token);
        Task SendForgetPasswordEmail(string email, string encodedToken, string clientUri);
    }
}
