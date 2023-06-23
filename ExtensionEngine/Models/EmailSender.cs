using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionEngine.Models
{
    class EmailSender
    {
        string emailServer, mPort;

        public EmailSender() { }

        public string SendEmail(string sender, string recipient, string EmSuj, string EmMsg)
        {
            string sent = "Succeed";
            SmtpClient sendmail = new SmtpClient();
            try
            {
                sendmail.Host = ConfigurationManager.AppSettings["EmailServer"];
                sendmail.Send(sender, recipient, EmSuj, EmMsg);
                return sent;
            }
            catch (SmtpFailedRecipientException ex)
            {
                return ex.Message;
            }
        }
    }
}
