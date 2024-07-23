using Application.Contracts;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Repositories
{
    public class MessageSender : IMessageSender
    {
        public void SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("danateldow2021@gmail.com", "ffxhhxcwmtleoykj"),
                EnableSsl = true,
            };

            smtpClient.Send("danateldow2021@gmail.com", toEmail, subject, message);

        }
    }
}
