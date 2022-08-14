namespace Spyro.Security
{
    public interface ICaptchaService
    {
        string Generate(string key, int length);
        bool Validate(string key, string value);
    }
}