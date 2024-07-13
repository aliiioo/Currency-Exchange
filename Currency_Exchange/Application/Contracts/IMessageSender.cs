using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IMessageSender
    {
        public void SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }





}
