using System.Net;
using System.Net.Mail;

namespace doe.rapido.api.Services
{
    public class MailService : IMailService
    {
        private string smtpAddress => "smtp.gmail.com";
        private int portNumber => 587;

        private string emailFromAddress => Settings.loginGmail;

        private string password => Settings.passwordGmail;

        public void sendEmail(string[] emails, string subject, string body, bool isHtml = false)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(emailFromAddress);
                AddEmailsToMessage(mailMessage, emails);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtml;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.Send(mailMessage);
                }
            }
        }

        private void AddEmailsToMessage(MailMessage mailMessage, string[] emails)
        {
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }
        }
    }
}
