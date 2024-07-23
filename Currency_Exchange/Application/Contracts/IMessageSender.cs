namespace Application.Contracts
{
    public interface IMessageSender
    {
        public void SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }





}
