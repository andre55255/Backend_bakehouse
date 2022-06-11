using AutoMapper;
using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Helpers;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.RepositoriesImpl
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogRepository _logRepo;

        public ApplicationUserRepository(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<int>> roleManager, ILogRepository logRepo)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logRepo = logRepo;
        }

        public async Task<Result> CheckIsEmailConfirmedAsync(ApplicationUser user)
        {
            try
            {
                bool result = await _signInManager
                                        .UserManager
                                        .IsEmailConfirmedAsync(user);

                if (result)
                    return Result.Ok();
                else
                    return Result.Ok().WithSuccess("F");
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                    ConstantsMessageUsers.ErrorCheckEmailConfirmed, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorCheckEmailConfirmed);
            }
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmAccountVO model)
        {
            try
            {
                ApplicationUser user = await FindByIdAsync(model.UserId);

                if (user == null)
                    return Result.Fail(ConstantsMessageUsers.ErrorUserNotFound);

                IdentityResult result = await _signInManager
                                                .UserManager
                                                .ConfirmEmailAsync(user, model.Code);

                if (result.Succeeded)
                    return Result.Ok();

                return Result.Fail(ConstantsMessageUsers.ErrorErrorConfirmEmailCode);
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                    ConstantsMessageUsers.ErrorErrorConfirmEmail, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorErrorConfirmEmail);
            }
        }

        public async Task<Result> CreateUserDbAsync(ApplicationUser user, List<string> roles)
        {
            try
            {
                user.Id = 0;
                user.UserName = user.UserName;
                user.NormalizedUserName = user.UserName.ToUpper();
                user.Email = user.Email;
                user.NormalizedEmail = user.Email.ToUpper();
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                user.DisabledAt = null;

                IdentityResult resultCreateUser = await _signInManager
                                                            .UserManager
                                                            .CreateAsync(user, user.PasswordHash);

                if (!resultCreateUser.Succeeded)
                    return Result.Fail(ConstantsMessageUsers.ErrorCreateUserDB);

                foreach (string role in roles)
                {
                    IdentityResult resultAddRoleToUser = await _signInManager
                                                                    .UserManager
                                                                    .AddToRoleAsync(user, role.ToUpper());

                    if (!resultAddRoleToUser.Succeeded)
                    {
                        _logRepo.Write(resultAddRoleToUser.Errors.FirstOrDefault().Code + ":" +
                                                    resultAddRoleToUser.Errors.FirstOrDefault().Description,
                                             ConstantsMessageUsers.ErrorAddRoleToUser + "Role:" + role.ToUpper() +
                                                    ", User" + user.Name, this.GetType().ToString());
                    }
                }

                return Result.Ok().WithSuccess(user.Id.ToString());
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                    ConstantsMessageUsers.ErrorCreateUser, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorCreateUser);
            }
        }

        public async Task<Result> DisableUserAsync(ApplicationUser user)
        {
            try
            {
                ApplicationUser save = await FindByIdAsync(user.Id);
                save.DisabledAt = DateTime.Now;

                IdentityResult result = await _signInManager
                                                .UserManager
                                                .UpdateAsync(save);

                if (result.Succeeded)
                    return Result.Ok();

                return Result.Fail(ConstantsMessageUsers.ErrorDisableUserDB);
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorDisableUserDB, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorDisableUserDB);
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string emailUser)
        {
            try
            {
                ApplicationUser user = await _signInManager.UserManager
                                                           .Users
                                                           .Where(x => x.DisabledAt == null &&
                                                                       x.Email == emailUser)
                                                           .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorFindUserByEmail, this.GetType().ToString());
                return null;
            }
        }

        public async Task<ApplicationUser> FindByIdAsync(int id)
        {
            try
            {
                ApplicationUser user = await _signInManager
                                                .UserManager
                                                .Users
                                                .Where(x => x.DisabledAt == null &&
                                                            x.Id == id)
                                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGetById, this.GetType().ToString());
                return null;
            }
        }

        public async Task<ApplicationUser> FindByUserNameAsync(string userName)
        {
            try
            {
                ApplicationUser user = await _signInManager
                                                .UserManager
                                                .Users
                                                .Where(x => x.DisabledAt == null &&
                                                            x.NormalizedUserName == userName.ToUpper())
                                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGetById, this.GetType().ToString());
                return null;
            }
        }

        public async Task<Result> GenerateRefreshTokenUserAsync(ApplicationUser user)
        {
            try
            {
                string refreshToken = await _signInManager
                                                .UserManager
                                                .GenerateUserTokenAsync(user, "JwtRefreshToken", "RefreshToken");

                await _signInManager
                         .UserManager
                         .SetAuthenticationTokenAsync(user, "JwtRefreshToken", "RefreshToken", refreshToken);

                return Result.Ok().WithSuccess(refreshToken);
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGenerateRefreshToken, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorGenerateRefreshToken);
            }
        }

        public async Task<string> GenerateTokenConfirmationEmailUserAsync(ApplicationUser user)
        {
            try
            {
                string code = await _signInManager
                                        .UserManager
                                        .GenerateEmailConfirmationTokenAsync(user);

                return code;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGenerateTokenConfirmEmail, this.GetType().ToString());
                return null;
            }
        }

        public async Task<string> GenerateTokenResetPasswordAsync(ApplicationUser user)
        {
            try
            {
                string codeResult =
                    await _signInManager
                                .UserManager
                                .GeneratePasswordResetTokenAsync(user);

                return codeResult;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGenerateTokenResetPassword, this.GetType().ToString());
                return null;
            }
        }

        public async Task<List<IdentityRole<int>>> GetAllRolesAsync()
        {
            try
            {
                List<IdentityRole<int>> rolesSaves =
                    await _roleManager
                             .Roles
                             .ToListAsync();

                return rolesSaves;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorFindAllRoles, this.GetType().ToString());

                return null;
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                List<ApplicationUser> resp = await _signInManager
                                                        .UserManager
                                                        .Users
                                                        .Where(x => x.DisabledAt == null)
                                                        .ToListAsync();

                return resp;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                    ConstantsMessageUsers.ErrorGetAllUsers, this.GetType().ToString());

                return null;
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsersByRolesAsync(string role)
        {
            try
            {
                IList<ApplicationUser> users = await _signInManager
                                                       .UserManager
                                                       .GetUsersInRoleAsync(role.ToUpper());

                return users.ToList();
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGetAll, this.GetType().ToString());

                return null;
            }
        }

        public async Task<List<string>> GetRolesFromUserAsync(ApplicationUser user)
        {
            try
            {
                IEnumerable<string> roles = await _signInManager.UserManager
                                                                .GetRolesAsync(user);

                List<string> roleList = roles.ToList();
                return roleList;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorFindRoleByUser, this.GetType().ToString());
                return null;
            }
        }

        public async Task<string> GetTokenAuthenticationAsync(ApplicationUser user)
        {
            try
            {
                string refreshToken = await _signInManager
                                                .UserManager
                                                .GetAuthenticationTokenAsync(user, "JwtRefreshToken", "RefreshToken");

                return refreshToken;
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorGetAuthToken, this.GetType().ToString());
                return null;
            }
        }

        public async Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string password)
        {
            try
            {
                IdentityResult result =
                    await _signInManager
                                .UserManager
                                .ResetPasswordAsync(user, token, password);

                if (result.Succeeded)
                    return Result.Ok();
                else
                    return Result.Fail("Error reset password");
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorResetPassword, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorResetPassword);
            }
        }

        public async Task<Result> ResetPasswordUserSignedAsync(ApplicationUser user, string newPassword)
        {
            try
            {
                ApplicationUser save = await FindByIdAsync(user.Id);

                PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
                save.PasswordHash = hasher.HashPassword(save, newPassword);
                save.UpdatedAt = DateTime.Now;

                IdentityResult resultEdit = await _signInManager
                                                    .UserManager
                                                    .UpdateAsync(save);

                if (resultEdit.Succeeded)
                    return Result.Ok();

                return Result.Fail(ConstantsMessageUsers.ErrorUpdateUserDB);
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorResetPassword, this.GetType().ToString());

                return Result.Fail(ConstantsMessageUsers.ErrorResetPassword);
            }
        }

        public async Task<Result> SignInUserAsync(ApplicationUser user, string password)
        {
            try
            {
                SignInResult result =
                    await _signInManager
                                .PasswordSignInAsync(
                                    user,
                                    password,
                                    false,
                                    false
                                );

                if (result.Succeeded)
                    return Result.Ok();
                else
                    return Result.Fail("Login attempt failed");
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorLogInto, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorLogInto);
            }
        }

        public async Task UpdateSecurityStampUserAsync(ApplicationUser user)
        {
            try
            {
                await _signInManager
                           .UserManager
                           .UpdateSecurityStampAsync(user);
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorUpdateUser, this.GetType().ToString());
            }
        }

        public async Task<Result> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                ApplicationUser userSave = await FindByIdAsync(user.Id);
                userSave.Name = user.Name;
                userSave.LastName = user.LastName;
                userSave.PhoneNumber = user.PhoneNumber;

                IdentityResult updateResult = await _signInManager
                                                        .UserManager
                                                        .UpdateAsync(userSave);

                if (!updateResult.Succeeded)
                    return Result.Fail(ConstantsMessageUsers.ErrorUpdateUserDB);

                List<string> rolesUser = await GetRolesFromUserAsync(userSave);
                IdentityResult removeRolesResult = await _signInManager
                                                           .UserManager
                                                           .RemoveFromRolesAsync(userSave, rolesUser);

                if (!removeRolesResult.Succeeded)
                    return Result.Fail(ConstantsMessageUsers.ErrorRemoveRolesByUser);

                foreach (string role in rolesUser)
                {
                    IdentityResult resultAddRoleToUser = await _signInManager
                                                                    .UserManager
                                                                    .AddToRoleAsync(userSave, role.ToUpper());

                    if (!resultAddRoleToUser.Succeeded)
                    {
                        _logRepo.Write(resultAddRoleToUser.Errors.FirstOrDefault().Code + ":" +
                                                    resultAddRoleToUser.Errors.FirstOrDefault().Description,
                                             ConstantsMessageUsers.ErrorAddRoleToUser + "Role:" + role.ToUpper() +
                                                    ", User" + user.Name, this.GetType().ToString());
                    }
                }

                return Result.Ok().WithSuccess(userSave.Id.ToString());
            }
            catch (Exception ex)
            {
                _logRepo.Write(ex.Message,
                   ConstantsMessageUsers.ErrorUpdateUser, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorUpdateUser);
            }
        }
    }
}