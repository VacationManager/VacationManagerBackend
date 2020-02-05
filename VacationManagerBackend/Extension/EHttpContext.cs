using Microsoft.AspNetCore.Http;

namespace VacationManagerBackend.Extension
{
    public static class EHttpContext
    {
        public static string GetAccessToken(this HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            string[] parts = authHeader.Split(' ');

            if (parts != null && parts.Length > 1)
                return parts[1];

            return null;
        }
    }
}
