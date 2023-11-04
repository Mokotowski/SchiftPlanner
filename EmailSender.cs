using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace SchiftPlanner
{
    public class EmailSender : IEmailSender
    {

        SmtpSettings _smtpSettings = new SmtpSettings
        {
            SmtpServer = "smtp-relay.sendinblue.com",
            Port = 587,
            Username = "dawiidtomaszewski354@gmail.com",
            Password = "vNYKkbdV4BOjfCxL",
            EnableSsl = true,
            SenderEmail = "dawiidtomaszewski354@gmail.com",
            SenderName = "SchiftPlanner"
        };




        public async Task SendEmailAsync(string email, string subject, string message)
        {

            var smtpClient = new SmtpClient(_smtpSettings.SmtpServer)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

}
