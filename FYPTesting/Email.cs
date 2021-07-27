using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPTesting
{
    public class Email
    {
        private readonly string from = "1422195260@outlook.com";
        private readonly string password = "Q8259866q";


        public void SendMessage(string to, string subject)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress(from));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = "PO has been accepted"
            };

            using(var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp-mail.outlook.com", 587);

                client.Authenticate(from, password);

                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}
