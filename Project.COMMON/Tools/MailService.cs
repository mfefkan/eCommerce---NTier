using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project.COMMON.Tools
{
    public static class MailService
    {


        public static void Send(string reciever,string password="ugur1006",string body = "Test mesajıdır",string subject ="Email testi",string sender="mfefkan96@gmail.com")
        {
            MailAddress senderEmail = new MailAddress(sender);
            MailAddress recieverMail = new MailAddress(reciever);

            //Email işlemleri SMTP'ye göre yapılır...
            //Mail adresinin bu şekilde kullanılabilmesi için 3. parti uygulamalar tarafından mail göndermeye açık olması gerekir.

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address,password)
            };

            using (MailMessage message = new MailMessage(senderEmail, recieverMail)
            {

                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }
    }
}
