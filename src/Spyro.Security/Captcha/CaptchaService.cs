using Spyro.Cache;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Spyro.Security
{
    public class CaptchaService : ICaptchaService
    {

        private readonly Random _random1 = new Random();
        private readonly ICacheProvider _cacheProvider;

        public CaptchaService(ICacheProvider cacheProvider) => _cacheProvider = cacheProvider;


        public string Generate(string key, int length)
        {
            var value = GenerateValue(length);
            var image = GenerateImage(value);
            _cacheProvider.Set(key, value, ExpireTime: DateTimeOffset.UtcNow.AddMinutes(2));
            return image;
        }
        public bool Validate(string key, string value)
        {
            var cacheValue = _cacheProvider.Get<string>(key);

            if (cacheValue == null)
                throw new ArgumentNullException(nameof(cacheValue));

            return string.Compare(cacheValue, value, true) == 0;
        }

        private string GenerateValue(int length)
        {
            string combination = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";
            StringBuilder captcha = new();
            for (var i = 0; i < length; i++)
                captcha.Append(combination[_random1.Next(combination.Length)]);
            return captcha.ToString();
        }
        private string GenerateImage(string value)
        {
            //TODO:Should migrate to ImageSharp in order to support crossplatform image building
            using MemoryStream memoryStream = new();
            Bitmap image = new(25 * value.Length, 50);
            RectangleF rectangleF = new(0, 0, image.Width, image.Height);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(value, new Font("Thaoma", 24, FontStyle.Italic), Brushes.Green, rectangleF);
            g.Flush();
            image.Save(memoryStream, ImageFormat.Png);
            g.Dispose();
            image.Dispose();
            return $"data:image/png;base64,{Convert.ToBase64String(memoryStream.GetBuffer())}"; ;
        }
    }
}
