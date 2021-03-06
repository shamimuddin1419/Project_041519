using Dos4PeopleApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Utility
{
    public class EmailService
    {      
        public EmailService()
        {
           
        }
        public async Task<int> PasswordRecovery(VmUser model, IWebHostEnvironment _webHostEnvironment)
        {
            int result = 0;
            string sender = "s@gmail.com";
            string receiver = model.Email;
            MailMessage Msg = new MailMessage();

            try
            {
                Msg.From = new MailAddress(sender);
                Msg.To.Add(receiver);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplate/PasswordRecovery.html"); 
                 StreamReader reader = new StreamReader(path);
                string readFile = reader.ReadToEnd();
                string StrContent = "";
                StrContent = readFile;
                StrContent = StrContent.Replace("[FullName]", model.FullName);
                StrContent = StrContent.Replace("[LoginID]", model.UserName);
                StrContent = StrContent.Replace("[LoginEmail]", model.Email);
                StrContent = StrContent.Replace("[Password]", model.Password);               
                Msg.Subject = model.UserName + " - Password Recovery";
                Msg.Body = StrContent.ToString();
                Msg.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "s@gmail.com",
                        Password = "6666666"
                    };
                    smtp.Credentials = credential;

                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;                 
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Timeout = 20000;              
                    await smtp.SendMailAsync(Msg);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = 0;
            }
            return result;
        }
        public void SendConfirmationMail()
        {
            //try
            //{
            string Email = "shamim.cse19@gmail.com";
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("noreply@dos4pp.net");
            mail.To.Add(Email);
            mail.Subject = "For testing";
            mail.Body = "";
            mail.IsBodyHtml = true;
            mail.Body = "<html>" +
                                "<body>" +
                                "<Table>" +
                                "<tr>" +
                                "<td>Hello</td>" +
                                "</tr>" +
                                "<tr>" +
                                "<td>Welcome to nexantrade.net </td>" +
                               
                                "<tr>" +                            
                                "</Table></body></html>";

            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Host = "mail.nexentrade.net";
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@dos4pp.net", "dos4ppPAss");
            SmtpServer.EnableSsl = false;

            SmtpServer.Send(mail);

            //}
            //catch (Exception ex)
            //{
            //    lblmsg.Text = ex.Message.ToString();
            //}
        }
    }
}
