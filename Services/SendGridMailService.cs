using AuthApp_Api.Services.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AuthApp_Api.Services
{
    public class SendGridMailService 
    {

        public IConfiguration Configuration { get; set; }
        public SendGridMailService(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            try
            {

                var apiKey = Configuration["SendGridAPIKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(Configuration["SenderEmail"], Msg.EmailMsgBody1);
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);


            }
            catch (Exception)
            {
                throw;
            }
        }
    }

   
}
