using System;

namespace Spyro.Security.GoogleAuthenticator
{
    public class GoogleAuthenticator : ITwoFactorAuthenticatorProvider
    {
        public string Generate(string key, int duration)
        {
            throw new NotImplementedException();
        }

        public bool Validate(string key, string code, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
