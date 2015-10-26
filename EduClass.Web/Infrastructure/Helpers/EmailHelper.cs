using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EduClass.Web.Infrastructure.Helpers
{
    public class EmailHelper
    {
        private string To { get; set; }
        private string Cc { get; set; }
        private string From { get; set; }
        private string Bcc { get; set; }
        private string Subject { get; set; }
        private string Body { get; set; }
        private string Server { get; set; }
        private int Port { get; set; }
        private bool EnableSSL { get; set; }
        private string User { get; set; }
        private string Password { get; set; }

        const string SERVER = "relay-hosting.secureserver.net";

        public EmailHelper() { }

        private void SendMail(string subject, string body)
        {
            /*this.To = ConfigurationManager.AppSettings.Get("mail_To");
            this.From = ConfigurationManager.AppSettings.Get("mail_From");
            this.Cc = ConfigurationManager.AppSettings.Get("mail_CC");
            this.Bcc = ConfigurationManager.AppSettings.Get("mail_BCC");
            this.Server = ConfigurationManager.AppSettings.Get("smtpServer");
            this.EnableSSL = Boolean.Parse(ConfigurationManager.AppSettings.Get("smtpEnableSSL"));
            this.Port = int.Parse(ConfigurationManager.AppSettings.Get("smtpPort"));
            this.User = ConfigurationManager.AppSettings.Get("smtpUser");
            this.Password = ConfigurationManager.AppSettings.Get("smtpPassword");
            this.Subject = subject;
            this.Body = body;

            if (String.IsNullOrEmpty(To) || String.IsNullOrEmpty(Server) || String.IsNullOrEmpty(User) || String.IsNullOrEmpty(Body))
            {
                throw new Exception("ERROR Sending Email - Set Email configuration");
            }

            var smtpServer = new SmtpClient
            {
                Host = Server,
                Port = Port,
                EnableSsl = EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(User, Password)
            };

            var mail = new MailMessage { From = this.,From };

            //var to = To.Split(char.Parse(";")); //Array de string con direcciones de mail

            mail.To = this.To;


            var htmlView = "<html><body>" + this.Body + "<hr/><table><tr width='100%'><td>Enviado desde Sakura Mailing</td></tr><tr width='100%'><td></td></tr></table></body></html>";

            //var alternateView = AlternateView.CreateAlternateViewFromString(htmlView, null, MediaTypeNames.Text.Html);

            mail.Subject = this.Subject;

            mail.BodyFormat = MailFormat.Html;
            mail.Priority = MailPriority.Normal;
            mail.Body = htmlView;
            //mail.AlternateViews.Add(alternateView);
            //mail.IsBodyHtml = true;

            SmtpMail.SmtpServer = SERVER;
            //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            SmtpMail.Send(mail);
            mail = null;*/
        }
    }
}