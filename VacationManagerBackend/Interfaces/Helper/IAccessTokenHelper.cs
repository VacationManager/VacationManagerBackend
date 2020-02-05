using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Helper
{
    public interface IAccessTokenHelper
    {
        string GenerateAccessToken(AccessTokenPayload tokenPayload);
    }
}
