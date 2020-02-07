using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Helper
{
    public interface IMailHelper
    {
        void SendMail(User user, string subject, string body, bool bodyAsHtml = false);
        void SendVacationRequestStateMail(User user, VacationRequest request);
    }
}
