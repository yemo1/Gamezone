using GameZone.VIEWMODEL;
using GameZone.TOOLS;
using System;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
namespace GameZone.TOOLS
{
    public class Notification
    {
        // Initialize SMTP Credentials from config
        private string senderAddress = ConfigurationManager.AppSettings["EMAIL_SENDER"].ToString();
        private string netPassword = ConfigurationManager.AppSettings["EMAIL_PW"].ToString();
        private string clientAddress = ConfigurationManager.AppSettings["SMTP_CLIENT"].ToString();
        private string clientPORT = ConfigurationManager.AppSettings["SMTP_CLIENT_PORT"].ToString();
        //string emailFrom = "postmaster@brightandwhitedrycleaners.com", emailTo = "info@brightandwhitedrycleaners.com", emailPW = "bright@2017";
        public bool SendEMail(string recipient, string subject, string message)
        {
            bool isMessageSent = false;
            try
            {
                RegexUtilities rU = new RegexUtilities();
                if (!rU.IsValidEmail(recipient))
                {
                    return isMessageSent;
                }
                //Intialise Parameters  
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(clientAddress);
                client.Port = int.Parse(clientPORT);
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(senderAddress, netPassword);
                client.EnableSsl = true;
                client.Credentials = credentials;

                var mail = new System.Net.Mail.MailMessage(senderAddress.Trim(), recipient.Trim());
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                //System.Net.Mail.Attachment attachment;  
                //attachment = new Attachment(@"C:\Users\XXX\XXX\XXX.jpg");  
                //mail.Attachments.Add(attachment);  
                client.Send(mail);
                isMessageSent = true;
            }
            catch (Exception ex)
            {
                isMessageSent = false;
                throw ex;
            }
            return isMessageSent;
        }
    }
}