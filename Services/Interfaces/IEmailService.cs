namespace lentynaBackEnd.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent);
        Task SendNewBookNotificationAsync(string toEmail, string toName, string authorName, string bookTitle);
    }
}
