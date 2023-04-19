namespace AuthApp_Api.Services.Interface
{
    public interface IEmailService
    {


        Task SendEmail(string To, string subject, string Body);

        
    }
}
