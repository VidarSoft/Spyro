using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Spyro.Security.Otp
{
    public class TotpGenerator
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);



        public string Generate(byte[] key, int step = 30, int digits = 6)
            => Generate(key, GetCurrentCounter(DateTime.UtcNow, step), digits);
        private string Generate(byte[] key, long iterationNumber, int digits = 6)
        {

            var counter = BitConverter.GetBytes(iterationNumber);
            if (BitConverter.IsLittleEndian) Array.Reverse(counter);

            var hmac = new HMACSHA1(key);
            var hmacComputedHash = hmac.ComputeHash(counter);

            int offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0x0F;
            var otp = (hmacComputedHash[offset] & 0x7f) << 24
                   | (hmacComputedHash[offset + 1] & 0xff) << 16
                   | (hmacComputedHash[offset + 2] & 0xff) << 8
                   | (hmacComputedHash[offset + 3] & 0xff) % 1000000;


            var result = otp % (int)Math.Pow(10, digits);
            return result.ToString().PadLeft(digits, '0');
        }
        public bool Validate(byte[] key, string code, int step, TimeSpan? timeTolerance = default) => GetCurrentOtps(key, step, timeTolerance ?? TimeSpan.FromSeconds(step)).Any(c => c == code);


        public long GetCurrentCounter() => GetCurrentCounter(DateTime.UtcNow, 30);
        public long GetCurrentCounter(DateTime timestamp, int step) => (long)(timestamp - _epoch).TotalSeconds / step;
        private string[] GetCurrentOtps(byte[] key, int step, TimeSpan timeTolerance)
        {
            var codes = new List<string>();
            var iterationCounter = GetCurrentCounter(DateTime.UtcNow, step);
            var iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30)
            {
                iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30);
            }

            var iterationStart = iterationCounter - iterationOffset;
            var iterationEnd = iterationCounter + iterationOffset;

            for (var counter = iterationStart; counter <= iterationEnd; counter++)
            {
                codes.Add(Generate(key, counter));
            }

            return codes.ToArray();
        }
    }
}
