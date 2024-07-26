using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace LucidHR.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _senderName = "Your Name";
        private readonly string _senderEmail = "prathamyrao47@gmail.com";
        private readonly string _username = "prathamyrao47@gmail.com";
        private readonly string _password = "npsxpsbhhoqqmpgc";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_senderName, _senderEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachment, string attachmentName)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_senderName, _senderEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            var multipart = new Multipart("mixed");
            var textPart = new TextPart(TextFormat.Html) { Text = body };
            multipart.Add(textPart);

            var attachmentPart = new MimePart("application", "pdf")
            {
                Content = new MimeContent(new MemoryStream(attachment)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentName
            };
            multipart.Add(attachmentPart);

            emailMessage.Body = multipart;

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
