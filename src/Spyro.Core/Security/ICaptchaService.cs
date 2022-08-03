namespace Spyro.Security
{
    public interface ICaptchaService
    {
        string Generate();
        bool Validate(string value);
    }
}