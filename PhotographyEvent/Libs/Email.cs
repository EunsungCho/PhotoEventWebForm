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
    
    // Class for handling email function
    class Email
    {
        /// <summary>
        /// sends email to designated user with given message
        /// </summary>
        /// <param name="Subject">subject of mail</param>
        /// <param name="Mail">body contents to send</param>
        /// <param name="Email">receiver's email</param>
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
                
            }
        }
    }
}
