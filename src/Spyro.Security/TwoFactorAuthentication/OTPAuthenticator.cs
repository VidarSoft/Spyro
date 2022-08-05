using Spyro.Security.Otp;
using System;
using System.Text;

namespace Spyro.Security
{
    public class OtpAuthenticator : ITwoFactorAuthenticatorProvider
    {
        private readonly TotpGenerator _generator;

        public OtpAuthenticator(TotpGenerator generator)
        {
            _generator = generator;
        }

        public string Generate(string key, int duration) => _generator.Generate(GetBytes(key), duration);
        public bool Validate(string key, string code, int duration) => _generator.Validate(GetBytes(key), code, duration);


        private byte[] GetBytes(string s) => Encoding.UTF8.GetBytes(s);
    }
}