using MailKit.Net.Smtp;
using MimeKit;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config) => _config = config;

    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_config["Email:From"]));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["Email:Host"],
            int.Parse(_config["Email:Port"]!), false);
        await smtp.AuthenticateAsync(_config["Email:User"],
            _config["Email:Pass"]);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}