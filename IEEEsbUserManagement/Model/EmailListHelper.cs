using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.Model
{
    public class EmailListHelper
    {
        Email email;

        public EmailListHelper(PropertiesFileHandler properties)
        {
            email = properties.RetrieveEmailInfo("EmailServer");
        }

        public bool SendWelcomeEmail(User user)
        {
            bool success = true;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(email.SmtpServer, Int32.Parse(email.Port));

                mail.From = new MailAddress(email.Sender);
                mail.To.Add(user.Email);
                mail.Subject = email.About;
                mail.Body = email.Content.Replace("%ID%", user.RegistrationID);


                SmtpServer.UseDefaultCredentials = false;

                SmtpServer.Credentials = new System.Net.NetworkCredential(email.Username, email.Password);

                SmtpServer.Send(mail);
            }
            catch(Exception e)
            {
                success = false;
                LogConnection.LogError(LogConnection.SUBTYPE.EMAIL, e.Message);
            }
            return success;
        }

        public bool SendIDRegenEmail(string newRegistrationID, string receipt)
        {
            bool success = true;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(email.SmtpServer, Int32.Parse(email.Port));

                mail.From = new MailAddress(email.Sender);
                mail.To.Add(receipt);
                mail.Subject = "Regeneración de token de sesión IEEEsb";
                mail.Body = "Su nuevo ID de registro es: " + newRegistrationID;


                SmtpServer.UseDefaultCredentials = false;

                SmtpServer.Credentials = new System.Net.NetworkCredential(email.Username, email.Password);

                SmtpServer.Send(mail);
            }
            catch(Exception e)
            {
                success = false;
                LogConnection.LogError(LogConnection.SUBTYPE.EMAIL, e.Message);
            }
            return success;
        }
    }
}
