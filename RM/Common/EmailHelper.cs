using System.Configuration;
using System.Net.Configuration;
using System.Web.Mail;

namespace RM.Common
{
    public static class EmailHelper
    {
        public static string SendEmail(string toEmail, string subject, string body, bool IsBodyHtml = true, string fromEmail = "")
        {
            try
            {
                SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                var fromEmailId = !string.IsNullOrEmpty(fromEmail) ? fromEmail : smtpSection.From;
                var mm = new MailMessage();
                mm.From = fromEmailId;
                mm.To = toEmail;
                mm.Subject = subject.Trim();
                mm.Body = body.Trim();
                mm.BodyFormat = MailFormat.Html;
                mm.Priority = MailPriority.High;
                mm.Body = body;

                System.Web.Mail.SmtpMail.SmtpServer = "relay-hosting.secureserver.net";
                System.Web.Mail.SmtpMail.Send(mm);
                return string.Empty;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            
            //using (MailMessage mm = new MailMessage(smtpSection.From, toEmail.Trim()))
            //{
            //    mm.Subject = subject.Trim();
            //    mm.Body = body.Trim();
            //    mm.IsBodyHtml = IsBodyHtml;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = smtpSection.Network.Host;
            //    //smtp.EnableSsl = smtpSection.Network.EnableSsl;
            //    //NetworkCredential networkCred = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
            //    //smtp.UseDefaultCredentials = smtpSection.Network.DefaultCredentials;
            //    //smtp.Credentials = networkCred;
            //    smtp.Port = smtpSection.Network.Port;
            //    smtp.Send(mm);
            //}
        }
    }
}