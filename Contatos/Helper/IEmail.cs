using System.Net.Mail;

namespace Contatos.Helper
{
    public interface IEmail
    {

        bool SendEmail(List<string> emailsTo, string subject, string body);
        MailMessage PrepareteMessage(List<string> emailsTo, string subject, string body);
        bool ValidateEmail(string email);
        bool SendEmailBySmtp(MailMessage message);

        
    }
}
