namespace InventoryApp.Services
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
