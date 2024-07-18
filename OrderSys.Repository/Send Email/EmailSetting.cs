
using System.Net.Mail;
using System.Net;

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
