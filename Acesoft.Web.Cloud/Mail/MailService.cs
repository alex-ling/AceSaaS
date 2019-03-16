using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Acesoft.Web.Cloud.Mail
{
    public class MailService : IMailService
    {
        private MailConfig mailConfig;

        public MailService(MailConfig mailConfig)
        {
            this.mailConfig = mailConfig;
        }

        public void Send(string mailto, string subject, string body)
        {
            var mailFrom = new MailAddress(mailConfig.From, mailConfig.Sender);
            var mailTo = new MailAddress(mailto);
            var smtp = new SmtpClient(mailConfig.Host, mailConfig.Port)
            {
                EnableSsl = mailConfig.Ssl
            };
            if (mailConfig.UserName.HasValue() && mailConfig.Password.HasValue())
            {
                smtp.Credentials = new NetworkCredential(mailConfig.UserName, mailConfig.Password, mailConfig.Domain);
            }

            using (smtp)
            { 
                var mailMsg = new MailMessage(mailFrom, mailTo);
                mailMsg.Subject = subject;
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = body;
                mailMsg.SubjectEncoding = Encoding.UTF8;
                mailMsg.BodyEncoding = Encoding.UTF8;
                mailMsg.HeadersEncoding = Encoding.UTF8;
                smtp.Send(mailMsg);
            }
        }
    }
}
