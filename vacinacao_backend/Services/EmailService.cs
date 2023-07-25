using System.Net;
using System.Net.Mail;

namespace vacinacao_backend.Services {
    public class EmailService {

        private SmtpClient ConfigureSMTP() {
            var smtpClient = new SmtpClient {
                Host = "Host",
                Port = 123, // Porta no host
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("username", "password"),
                Timeout = 20000
            };

            return smtpClient;
        }

        private void BuildMailMessage() {
            var mailMessage = new MailMessage() {
                Subject = "Título",
                Body = "Body", // Pode ser HTML
                IsBodyHtml = true, // Caso o body seja HTML
                Priority = MailPriority.Normal,
                From = new MailAddress("email do remetente")
            };

            mailMessage.To.Add(new MailAddress("Destinatário(s)"));
            mailMessage.Bcc.Add(new MailAddress("Cópia(s) oculta(s)"));
        }

        private void SendEmail(MailMessage mailMessage) {
            var smtpClient = ConfigureSMTP();
            smtpClient.Send(mailMessage);
        }
    }
}
