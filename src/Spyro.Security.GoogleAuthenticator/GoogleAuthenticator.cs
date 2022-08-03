using System;

namespace Spyro.Security.GoogleAuthenticator
{
    public class GoogleAuthenticator : ITwoFactorAuthenticatorProvider
    {
        public string Generate()
        {
            throw new NotImplementedException();
        }

        public bool Validate(string value)
        {
            throw new NotImplementedException();
        }
    }
}
