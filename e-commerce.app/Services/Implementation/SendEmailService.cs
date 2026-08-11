using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

namespace e_commerce.app.Services.ExternalService
{
    public interface ISendEmailService
    {
        // 1. الدالة بقت Async وبتاخد CancellationToken
        Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default);
    }

    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _config;

        public SendEmailService(IConfiguration config)
        {
            _config = config; 
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            
            
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls, ct);
            await smtp.AuthenticateAsync(_config["EmailSettings:FromEmail"], _config["EmailSettings:AppPassword"], ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }
}