using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace e_commerce.app.Services.ExternalService
{

    public class SendEmailService
    {
        private readonly string _fromEmail = "mahmouddiab6600@gmail.com";
        private readonly string _appPassword = "syft funs xtjj usic";

        public void SendEmail(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("mahmouddiab6600@gmail.com", _appPassword);
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}

