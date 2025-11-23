using lentynaBackEnd.Services.Interfaces;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace lentynaBackEnd.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly TransactionalEmailsApi? _emailApi;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly bool _isConfigured;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;

            var apiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");
            _isConfigured = !string.IsNullOrEmpty(apiKey);

            if (!_isConfigured)
            {
                _logger.LogInformation("BREVO_API_KEY is not configured. Email sending is disabled.");
                _emailApi = null;
            }
            else
            {
                Configuration.Default.ApiKey["api-key"] = apiKey;
                _emailApi = new TransactionalEmailsApi();
                _logger.LogInformation("Email service initialized with Brevo API.");
            }

            _senderEmail = Environment.GetEnvironmentVariable("BREVO_SENDER_EMAIL") ?? "noreply@lentyna.lt";
            _senderName = Environment.GetEnvironmentVariable("BREVO_SENDER_NAME") ?? "Lentyna";
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent)
        {
            if (!_isConfigured || _emailApi == null)
            {
                _logger.LogDebug("Email skipped (not configured). To: {ToEmail}, Subject: {Subject}", toEmail, subject);
                return;
            }

            try
            {
                var sendSmtpEmail = new SendSmtpEmail
                {
                    Sender = new SendSmtpEmailSender(_senderName, _senderEmail),
                    To = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(toEmail, toName) },
                    Subject = subject,
                    HtmlContent = htmlContent
                };

                var result = await _emailApi.SendTransacEmailAsync(sendSmtpEmail);
                _logger.LogInformation("Email sent successfully to {ToEmail}. MessageId: {MessageId}", toEmail, result.MessageId);
            }
            catch (ApiException ex) when (ex.Message.Contains("unauthorized"))
            {
                _logger.LogWarning("Invalid Brevo API key. Email not sent to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
            }
        }

        public async Task SendNewBookNotificationAsync(string toEmail, string toName, string authorName, string bookTitle)
        {
            var subject = $"Naujas {authorName} kurinys - {bookTitle}";

            var htmlContent = GetNewBookEmailTemplate(toName, authorName, bookTitle);

            await SendEmailAsync(toEmail, toName, subject, htmlContent);
        }

        private static string GetNewBookEmailTemplate(string toName, string authorName, string bookTitle)
        {
            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4a5568; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background-color: #f7fafc; padding: 30px; border-radius: 0 0 8px 8px; }}
        .book-title {{ color: #2d3748; font-size: 24px; font-weight: bold; margin: 20px 0; }}
        .author-name {{ color: #4a5568; font-size: 18px; }}
        .button {{ display: inline-block; background-color: #4299e1; color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; margin-top: 20px; }}
        .footer {{ text-align: center; margin-top: 20px; color: #718096; font-size: 12px; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Lentyna</h1>
        </div>
        <div class=""content"">
            <p>Sveiki, {toName}!</p>
            <p>Puikios naujienos! Autorius, kuri sekate, isleido nauja knyga:</p>
            <p class=""book-title"">{bookTitle}</p>
            <p class=""author-name"">Autorius: {authorName}</p>
            <p>Apsilankykite Lentyna svetaineje, kad suzinotumete daugiau apie sia knyga ir pridetumete ja i savo skaitymo sarasa.</p>
            <a href=""#"" class=""button"">Perziureti knyga</a>
        </div>
        <div class=""footer"">
            <p>Jus gavote si laiska, nes sekate autoriu {authorName} Lentyna svetaineje.</p>
            <p>© {DateTime.Now.Year} Lentyna - Knygu Vertinimo Sistema</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
