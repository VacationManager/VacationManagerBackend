using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using VacationManagerBackend.Enums;
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

        public void SendMail(User user, string subject, string body, bool bodyAsHtml = false)
        {
            _logger.Info("Send Mail...", new { user, subject, body, bodyAsHtml });

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential("noreply.vacationmanager@gmail.com", "NiklasKnoll99");
                smtpClient.EnableSsl = true;

                var mail = new MailMessage();
                mail.From = new MailAddress("noreply.vacationmanager@gmail.com");
                mail.To.Add(user.MailAddress);
                mail.Subject = subject;
                mail.Body = body;

                smtpClient.Send(mail);

                _logger.Info("Mail successfully send!", new { user, subject, body, bodyAsHtml });
            }

            catch (Exception ex)
            {
                _logger.Exception(ex);
            }
        }

        public void SendVacationRequestStateMail(User user, VacationRequest request)
        {
            _logger.Info("Send VacationRequestStateMail...", new { user, request });
            SendMail(user, $"Urlaubsantrag {(request.RequestState == VacationRequestState.Confirmed ? "bestätigt" : "abgelehnt")}", $"Dein Urlaubsantrag vom {request.StartTime.ToString("dd.MM.yyyy")} bis zum {request.EndTime.ToString("dd.MM.yyyy")} wurde {(request.RequestState == VacationRequestState.Confirmed ? "bestätigt" : "abgelehnt")}.");
            _logger.Info("VacationRequestStateMail successfully send!", new { user, request });
        }
    }
}
