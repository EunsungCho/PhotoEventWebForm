using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace PhotographyEvent.Libs
{
    class Email
    {
        public static void SendMailTo(string Subject, string Mail, string Email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("photographyenvet@gmail.com");
                mail.To.Add(Email);
                mail.Subject = Subject;
                mail.Body = Mail;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("photographyenvet@gmail.com", "Dmi@2017");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
    }
}
