namespace AuthApp_Api.Services.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);

    }
}
