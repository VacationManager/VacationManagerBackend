﻿using System;
using Trivial.Security;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Helper
{
    public class AccessTokenHelper : IAccessTokenHelper
    {
        const string secret = "DasISTeinMegaSICHERERSecretStRING";

        public string GenerateAccessToken(AccessTokenPayload tokenPayload)
        {
            var sign = HashSignatureProvider.CreateHS256(secret);
            var jwt = new JsonWebToken<AccessTokenPayload>(tokenPayload, sign);
            var str = jwt.ToEncodedString();

            return str;
        }

        public bool IsTokenValid(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrWhiteSpace(accessToken))
            {
                try
                {
                    var parser = new JsonWebToken<AccessTokenPayload>.Parser(accessToken);
                    var payload = parser.GetPayload();
                    var sign = HashSignatureProvider.CreateHS256(secret);
                    var isValid = parser.Verify(sign);

                    var remainingExpDurationS = (payload.ExpirationDate - DateTime.UtcNow).TotalSeconds;

                    if (isValid && remainingExpDurationS > 0)
                        return true;
                }

                catch
                {
                    return false;
                }
            }

            return false;
        }

        public AccessTokenPayload GetTokenPayload(string accessToken)
        {
            if (IsTokenValid(accessToken))
            {
                var parser = new JsonWebToken<AccessTokenPayload>.Parser(accessToken);
                return parser.GetPayload();
            }

            return null;
        }
    }
}
