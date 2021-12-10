namespace doe.rapido.api.Services
{
    public interface IMailService
    {
        void sendEmail(string[] emails, string subject, string body, bool isHtml = false);
    }
}
