using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace LucidHR.Controllers
{
    public class EmailSendController : Controller
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
       // private readonly HttpContext _httpContext;  

        public EmailSendController(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, string fromEmail )
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _fromEmail = fromEmail;
        }

        public async Task SendEmailAsync(List<string> toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                //
              //  string userEmail = HttpContext.Session.GetString("useremail");

                //if (!string.IsNullOrEmpty(userEmail) && MailAddress.TryCreate(userEmail, out var mailAddress))
                //{
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    foreach (var email in toEmail)
                    {
                        mailMessage.To.Add(email);
                    }

                    await client.SendMailAsync(mailMessage);
                //}
               
            }
        }
    }
}

