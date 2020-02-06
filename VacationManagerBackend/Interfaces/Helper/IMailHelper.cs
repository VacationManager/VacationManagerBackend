using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Helper
{
    public interface IMailHelper
    {
        void SendMail(User user);
    }
}
