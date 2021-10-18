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
            string sender = "mmm@gmail.com";
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
                StrContent = StrContent.Replace("[FullName]", model.UserName);
                StrContent = StrContent.Replace("[LoginID]", model.UserName);
                StrContent = StrContent.Replace("[LoginEmail]", model.Email);
                StrContent = StrContent.Replace("[Password]", model.Password);
                StrContent = StrContent.Replace("[RequestedIP]", model.RequestedIP);   
                Msg.Subject = model.UserName + " - Password Recovery";
                Msg.Body = StrContent.ToString();
                Msg.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "mm@gmail.com",
                        Password = "6555555"
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

    }
}
