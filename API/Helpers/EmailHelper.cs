using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace API.Helpers
{
    public class EmailHelper
    {
        public enum MessageType
        {
            Acknowledge, Accept, Reject, New
        }

        public async Task SendEmail(MessageType type, string body, string toAddress)
        {
            var builder = new BodyBuilder();
            string subject;
            builder.HtmlBody = body;

            switch (type)
            {
                case MessageType.Accept:
                    subject = "Your donation to House2Home was accepted!";
                    break;
                case MessageType.New:
                    subject = "New donation";
                    break;
                case MessageType.Reject:
                    subject = "Sorry, we can't accept your recent donation...";
                    break;
                case MessageType.Acknowledge:
                    subject = "Thanks for your donation offer - we'll get back to you soon!";
                    break;
                default:
                    builder.HtmlBody = "<P>ERROR: NO BODY ;(</p>";
                    subject = "NO SUBJECT";
                    break;
            }

            string username = Startup.Configuration.GetSection("Email:Username").Value;
            string password = Startup.Configuration.GetSection("Email:Password").Value;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Helping Hands", username));
            message.To.Add(new MailboxAddress(toAddress));
            message.Subject = subject;
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}