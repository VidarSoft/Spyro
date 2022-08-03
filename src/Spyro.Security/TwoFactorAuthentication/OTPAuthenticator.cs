using System;

namespace Spyro.Security
{
    public class OTPAuthenticator : ITwoFactorAuthenticatorProvider
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
