using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Helper
{
    public interface IAccessTokenProvider
    {
        AccessTokenPayload GetTokenPayload();
    }
}
