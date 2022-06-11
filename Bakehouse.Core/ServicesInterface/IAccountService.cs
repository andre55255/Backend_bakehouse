using Bakehouse.Communication.ViewObjects.Account;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IAccountService
    {
        public Task<TokenUserVO> LoginUser(LoginVO userVO, HttpRequest request);
        public Task<RefreshTokenVO> RefreshTokenUser(RefreshTokenVO model);
        Task<Result> ConfirmAccountUser(ConfirmAccountVO model);
    }
}