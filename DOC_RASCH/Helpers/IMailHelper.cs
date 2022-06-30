using DOC_RASCH.Common.Models;

namespace DOC_RASCH.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
