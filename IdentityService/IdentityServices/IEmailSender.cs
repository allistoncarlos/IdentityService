using System.Threading.Tasks;

namespace IdentityService.IdentityServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}