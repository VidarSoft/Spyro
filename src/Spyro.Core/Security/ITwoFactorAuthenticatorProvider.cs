using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyro.Security
{
    public interface ITwoFactorAuthenticatorProvider
    {
        string Generate(string key, int duration);
        bool Validate(string key, string code, int duration);
    }
}
