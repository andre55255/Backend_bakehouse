using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Core.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.RepositoriesInterface
{
    public interface IApplicationUserRepository
    {
        public Task<ApplicationUser> FindByEmailAsync(string emailUser);
        public Task<ApplicationUser> FindByUserNameAsync(string userName);
        public Task<Result> SignInUserAsync(ApplicationUser user, string password);
        public Task<string> GenerateTokenResetPasswordAsync(ApplicationUser user);
        public Task<List<string>> GetRolesFromUserAsync(ApplicationUser user);
        public Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string password);
        public Task<Result> CheckIsEmailConfirmedAsync(ApplicationUser user);
        public Task<Result> GenerateRefreshTokenUserAsync(ApplicationUser user);
        public Task<string> GetTokenAuthenticationAsync(ApplicationUser user);
        public Task UpdateSecurityStampUserAsync(ApplicationUser user);
        public Task<string> GenerateTokenConfirmationEmailUserAsync(ApplicationUser user);
        public Task<Result> CreateUserDbAsync(ApplicationUser user, List<string> roles);
        public Task<Result> ConfirmEmailAsync(ConfirmAccountVO model);
        public Task<ApplicationUser> FindByIdAsync(int id);
        public Task<Result> UpdateUserAsync(ApplicationUser user, List<string> roles);
        public Task<Result> DisableUserAsync(ApplicationUser user);
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task<List<IdentityRole<int>>> GetAllRolesAsync();
        public Task<Result> ResetPasswordUserSignedAsync(ApplicationUser user, string newPassword);
        public Task<List<ApplicationUser>> GetAllUsersByRolesAsync(string role);
    }
}