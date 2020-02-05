using Microsoft.AspNetCore.Http;
using VacationManagerBackend.Extension;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Helper
{
    public class AccessTokenProvider
    {
        IHttpContextAccessor _httpContextAccessor;
        IAccessTokenHelper _accessTokenHelper;

        public AccessTokenProvider(
            IHttpContextAccessor httpContextAccessor,
            IAccessTokenHelper accessTokenHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _accessTokenHelper = accessTokenHelper;
        }

        public AccessTokenPayload GetTokenPayload()
        {
            var at = _httpContextAccessor.HttpContext.GetAccessToken();
            return _accessTokenHelper.GetTokenPayload(at);
        }
    }
}
