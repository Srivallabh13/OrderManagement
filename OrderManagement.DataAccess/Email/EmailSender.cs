using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "srivallabhjoshi13@gmail.com";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, "vvriadlmiaevqsgf")
            };
            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
    }
}
