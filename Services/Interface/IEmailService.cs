using AuthApp_Api.Models;

namespace AuthApp_Api.Services.Interface
{
    public interface IEmailService
    {

        //void SendEmail(EmailDto request);
        Task SendEmail(string To, string subject, string Body);
    }
}
