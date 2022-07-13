using Bakehouse.Communication.ViewObjects.Account;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IAccountService
    {
        public Task<TokenUserVO> LoginUser(LoginVO userVO);
        public Task<RefreshTokenVO> RefreshTokenUser(RefreshTokenVO model);
        public Task<Result> ConfirmAccountUser(ConfirmAccountVO model);
        public Task<Result> ForgotPasswordSendMail(ForgotPasswordVO model);
        public Task<Result> ResetPasswordUser(ResetPasswordVO model);
        public Task<Result> ResetPasswordSignInUser(string idUser, string newPassword);
    }
}