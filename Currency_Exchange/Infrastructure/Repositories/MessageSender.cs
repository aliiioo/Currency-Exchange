using Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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
