using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using MkSmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TestSmtpMib
{
    class Program
    {

        static void Main(string[] args)
        {

            string sender = ConfigurationManager.AppSettings["Sender"];
            string recipient = ConfigurationManager.AppSettings["Recipient"];

            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress(sender);
            objMailMessage.To.Add(new MailAddress(recipient));
            objMailMessage.Subject = "test invio";
            objMailMessage.Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent aliquam leo augue, non laoreet dui placerat ac. Nam commodo leo sapien, eget sollicitudin mi porttitor in. Ut vehicula, odio ut sodales tempor, ipsum sapien aliquet purus, fermentum viverra sapien libero ac ligula.";

            sendWithDotNetComponent(objMailMessage);
            sendWithMailKit(objMailMessage);
        }

        static void sendWithDotNetComponent(MailMessage objMailMessage)
        {
            using (SmtpClient objSmtpClient = new SmtpClient())
            {
                try
                {

                    string usr = ConfigurationManager.AppSettings["User"];
                    string pwd = ConfigurationManager.AppSettings["Password"];
                    string domain = ConfigurationManager.AppSettings["Domain"];
                    string host = ConfigurationManager.AppSettings["Host"];
                    string taragetName = ConfigurationManager.AppSettings["TargetName"];
                    int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                    NetworkCredential credentials = new NetworkCredential(usr, pwd, domain);

                    objSmtpClient.UseDefaultCredentials = false;
                    objSmtpClient.Credentials = credentials;
                    objSmtpClient.Host = host;
                    objSmtpClient.TargetName = taragetName;
                    objSmtpClient.Port = port;
                    objSmtpClient.EnableSsl = true;
                    objSmtpClient.Send(objMailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
                finally
                {
                    objMailMessage = null;
                }
            }
        }

        static void sendWithMailKit (MailMessage objMailMessage)
        {

            using (MkSmtpClient client = new MkSmtpClient())
            {
                try
                {
                    var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(objMailMessage);

                    string usr = ConfigurationManager.AppSettings["User"];
                    string pwd = ConfigurationManager.AppSettings["Password"];
                    string domain = ConfigurationManager.AppSettings["Domain"];
                    string host = ConfigurationManager.AppSettings["Host"];
                    int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                    NetworkCredential credentials = new NetworkCredential(usr, pwd, domain);

                    client.Connect(host, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    client.Authenticate(credentials);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
                finally
                {
                    objMailMessage = null;
                }
            }
        }
    }
}
