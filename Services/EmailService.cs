using AuthApp_Api.Models;
using AuthApp_Api.Services.Interface;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Crypto.Macs;

namespace AuthApp_Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

    

    public async Task SendEmail(string To, string subject, string Body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = Body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls).ConfigureAwait(false);
            await smtp.AuthenticateAsync(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value).ConfigureAwait(false);
            await smtp.SendAsync(email).ConfigureAwait(false);
            await smtp.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}
