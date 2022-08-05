using QRCoder;
using Spyro.Core.Encoder;
using Spyro.Security.Otp;
using System;
using System.Linq;
using System.Text;

namespace Spyro.Security.GoogleAuthenticator
{

    public interface IGoogleAuthenticator
    {
        SetupConfig Generate(string issuer, string account, string key);

        bool Validate(string key, string code);

    }
    public class GoogleAuthenticator : IGoogleAuthenticator
    {

        public bool Validate(string key, string code) => TotpGenerator.Validate(GetBytes(key), code);


        public SetupConfig Generate(string issuer, string account, string key) => GenerateSetupCode(issuer, account, GetBytes(key));
        private SetupConfig GenerateSetupCode(string issuer, string account, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new NullReferenceException("Account is null");

            var normalizedAccount = NormalizeAccount(account);

            var encodedSecretKey = Base32Encoding.ToString(key).Trim('=');

            var provisionUrl = string.IsNullOrWhiteSpace(issuer)
                ? $"otpauth://totp/{normalizedAccount}?secret={encodedSecretKey}"
                : $"otpauth://totp/{UrlEncode(issuer)}:{normalizedAccount}?secret={encodedSecretKey}&issuer={UrlEncode(issuer)}";

            return new SetupConfig(normalizedAccount, encodedSecretKey, GenerateQrCodeUrl(provisionUrl));
        }
        private string NormalizeAccount(string s) => new string(Uri.EscapeDataString(s).Where(c => !char.IsWhiteSpace(c)).ToArray());
        private string UrlEncode(string s) => Uri.EscapeDataString(s);
        private string GenerateQrCodeUrl(string url)
        {
            string qrCodeUrl = default!;
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new PngByteQRCode(qrCodeData))
            {
                var qrCodeImage = qrCode.GetGraphic(3);
                qrCodeUrl = $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
            }

            return qrCodeUrl;
        }

        private byte[] GetBytes(string s, bool isBase32 = false) => isBase32 ? Base32Encoding.ToBytes(s) : Encoding.UTF8.GetBytes(s);
    }

    public record SetupConfig(string Account, string ManualSecretKey, string QrCodeUrl);
}
