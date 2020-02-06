using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Helper
{
    public class MailHelper : IMailHelper
    {
        ILogger _logger;

        public MailHelper(ILogger<MailHelper> logger)
        {
            _logger = logger;
        }

        public void SendMail(User user)
        {
            _logger.Info("Send Mail...", new { user });

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 25);
                smtpClient.Credentials = new NetworkCredential("niklas.knoll.64@gmail.com", "NiklasKnoll99");
                smtpClient.EnableSsl = true;

                var mail = new MailMessage();
                mail.From = new MailAddress("niklas.knoll@tobit.software");
                mail.To.Add(user.MailAddress);
                mail.Subject = "Urlaubsantrag bestätigt!";
                mail.Body = "Dein Urlaubsantrag wurde bestätigt!";

                smtpClient.Send(mail);

                _logger.Info("Mail successfully send!", new { user });
            }

            catch (Exception ex)
            {
                _logger.Exception(ex);
            }
        }
    }
}
