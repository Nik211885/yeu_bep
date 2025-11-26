using MimeKit;
using YeuBep.Extends.DataModel;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace YeuBep.Extends;

public class EmailSenderServices
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<EmailSenderServices> _logger;

    public EmailSenderServices(ILogger<EmailSenderServices> logger, MailSettings mailSettings)
    {
        _mailSettings = mailSettings;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string bodyHtml, string subject, string? nameTo = null)
    {
        try
        {
            var emailMessage = new MimeMessage();
            var emailForm = new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId);
            emailMessage.From.Add(emailForm);
            nameTo ??= to.Split("@")[0];
            var emailTo = new MailboxAddress(nameTo, to);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = bodyHtml
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
            await mailClient.AuthenticateAsync(_mailSettings.EmailId, _mailSettings.PasswordApplication);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);
            mailClient.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(@"Has problem with send email Exception 
                            with content send {to} and body is {body}
                            subject {subject}, name is {nameTo} {ex}",
                ex, to, bodyHtml,subject,nameTo);
        }
    }
}