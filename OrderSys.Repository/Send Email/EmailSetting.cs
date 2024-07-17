using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Send_Email
{
    public class EmailSetting
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ahmedKhaled.route@gmail.com", "ezsigncambdmlwcg");
            client.Send("ahmedKhaled.route@gmail.com", email.To, email.Title, email.Body);
        }
    }
}
