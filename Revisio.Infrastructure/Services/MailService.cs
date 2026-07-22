using MailKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Revisio.Infrastructure.Settings;
namespace Revisio.Infrastructure.Services
{
    public class MailService :Revisio.Application.Common.Interfaces.IMailService  
    {
        private readonly MailSetting mailSetting;
        private readonly IWebHostEnvironment webHostEnvironment;
        public MailService(IOptions<MailSetting> options,IWebHostEnvironment webHostEnvironment)
        {
            this.mailSetting = options.Value;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task SendEmailAsync(string mailTo, string? subject, List<IFormFile> attachments, string body)
        {
            var message = new MimeMessage();

            // From
            message.From.Add(new MailboxAddress("Revisio", mailSetting.Email));

            // To
            message.To.Add(new MailboxAddress("", mailTo));

            message.Subject = subject ?? string.Empty;

            var builder = new BodyBuilder();

            // Attachments
            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        builder.Attachments.Add(file.FileName, ms.ToArray());
                    }
                }
            }


            builder.HtmlBody = body;

            message.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(mailSetting.Host, mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(mailSetting.Email, mailSetting.Password);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
        public async Task SendConfirmationEmail(string fileName, string replaceWord, string url, string Email)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Templets", fileName);
            string content = File.ReadAllText(path);
            string finalContent = content.Replace(replaceWord, url);
            await SendEmailAsync(mailTo: Email, subject: "Revisio", null, finalContent);
        }
    }
}
