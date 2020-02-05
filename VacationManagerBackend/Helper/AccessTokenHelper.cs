using Trivial.Security;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Helper
{
    public class AccessTokenHelper : IAccessTokenHelper
    {
        public string GenerateAccessToken(AccessTokenPayload tokenPayload)
        {
            var sign = HashSignatureProvider.CreateHS256("DasISTeinMegaSICHERERSecretStRING");
            var jwt = new JsonWebToken<AccessTokenPayload>(tokenPayload, sign);
            var str = jwt.ToEncodedString();

            return str;
        }
    }
}
