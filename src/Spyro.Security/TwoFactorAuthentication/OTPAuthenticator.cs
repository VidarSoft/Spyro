using Spyro.Security.Otp;
using System;
using System.Text;

namespace Spyro.Security
{
    public class OtpAuthenticator : ITwoFactorAuthenticatorProvider
    {


        public string Generate(string key, int duration) => TotpGenerator.Generate(GetBytes(key), duration);
        public bool Validate(string key, string code, int duration) => TotpGenerator.Validate(GetBytes(key), code, duration);


        private byte[] GetBytes(string s) => Encoding.UTF8.GetBytes(s);
    }
}