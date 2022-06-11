using Bakehouse.Communication.ViewObjects.Email;
using FluentResults;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface ISendMailService
    {
        public Task<Result> SendMailUserConfirmation(EmailDataVO dataVO);
        public Task<Result> SendMailForgotPassword(EmailDataVO dataVO);
    }
}