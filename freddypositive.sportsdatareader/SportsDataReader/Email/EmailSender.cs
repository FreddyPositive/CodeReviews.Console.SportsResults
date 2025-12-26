using Microsoft.Extensions.Configuration;
using SportsDataReader.Logging;
using SportsDataReader.Model;
using System.Net.Mail;

namespace SportsDataReader.Email;

public class EmailSender
{
    private readonly IConfiguration _config;
    private readonly FileLogger _logger;

    public EmailSender(IConfiguration config)
    {
        _config = config;
        _logger = new FileLogger("Logs/app-log.txt");
    }

    public async Task SendSportsDataAsync(List<TeamStandingDto> eastStandings, List<TeamStandingDto> westStandings)
    {
        _logger.Info("Email generation started.");
        try
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"]);
            var username = _config["Smtp:Username"];
            var password = _config["Smtp:Password"];
            var enableSsl = bool.Parse(_config["Smtp:EnableSsl"]);
            var senderEmail = _config["Smtp:SenderEmail"];
            var receiverEmail = _config["Smtp:ReceiverEmail"];

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            EmailBodyFormatter emailBodyFormatter = new();
            var mailBody = emailBodyFormatter.BuildStandingsHtml(eastStandings, westStandings);
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "NBA Standings Report",
                Body = mailBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(receiverEmail);

            await smtpClient.SendMailAsync(mailMessage);
            _logger.Info("Email Sent succesfully.");
        }
        catch(Exception ex)
        {
            _logger.Error("Unhandled error occurred during email sending.", ex);
        }
    }
}
