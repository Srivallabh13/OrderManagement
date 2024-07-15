using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrderManagement.DataAccess.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "srivallabhjoshi13@gmail.com";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, "vvriadlmiaevqsgf")
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {email} with subject {subject}.");
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, $"SMTP error occurred while sending email to {email}.");
                throw new Exception("SMTP error occurred while sending email.", smtpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending email to {email}.");
                throw new Exception("An error occurred while sending email.", ex);
            }
        }
    }
}
