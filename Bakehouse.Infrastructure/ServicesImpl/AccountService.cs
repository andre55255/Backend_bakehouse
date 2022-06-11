using AutoMapper;
using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Communication.ViewObjects.Email;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class AccountService : IAccountService
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly ISendMailService _sendMailService;
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public AccountService(ILogService logService, IMapper mapper, IApplicationUserRepository userRepo, ISendMailService sendMailService, IConfiguration configuration)
        {
            _logService = logService;
            _mapper = mapper;
            _userRepo = userRepo;
            _sendMailService = sendMailService;
            _configuration = configuration;
        }

        public async Task<Result> ConfirmAccountUser(ConfirmAccountVO model)
        {
            try
            {
                Result result = await _userRepo.ConfirmEmailAsync(model);
                if (result.IsFailed)
                    return Result.Fail(result.Errors.FirstOrDefault().Message);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorErrorConfirmEmail, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorErrorConfirmEmail);
            }
        }

        public async Task<TokenUserVO> LoginUser(LoginVO userVO, HttpRequest request)
        {
            try
            {
                ApplicationUser user = await _userRepo.FindByUserNameAsync(userVO.UserName);
                if (user == null)
                    return new TokenUserVO { Error = ConstantsMessageUsers.ErrorUserNotFound };

                Result isEmailConfirmed = await _userRepo.CheckIsEmailConfirmedAsync(user);
                if (isEmailConfirmed.IsFailed)
                    return new TokenUserVO { Error = isEmailConfirmed.Errors.FirstOrDefault().Message };

                if (isEmailConfirmed.Successes.Count > 0 && isEmailConfirmed.Successes.FirstOrDefault().Message == "F")
                {
                    await SendEmailConfirmationAccount(user, request);
                    return new TokenUserVO { Error = ConstantsMessageUsers.ErrorUserNotConfirmed };
                }

                Result loginUser = await _userRepo.SignInUserAsync(user, userVO.Password);
                if (loginUser.IsSuccess)
                {
                    List<string> roleUser = await _userRepo.GetRolesFromUserAsync(user);

                    TokenUserVO token = await CreateTokenJwt(user, roleUser);

                    if (string.IsNullOrEmpty(token.Value))
                        return new TokenUserVO { Error = ConstantsMessageUsers.ErrorGenerateTokenJwt };

                    token.User = _mapper.Map<UserVO>(user);
                    token.User.Roles = String.Join(',', roleUser);

                    return token;
                }

                return new TokenUserVO { Error = ConstantsMessageUsers.ErrorLogInto };
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorLogInto, this.GetType().ToString());
                return new TokenUserVO { Error = ConstantsMessageUsers.ErrorLogInto };
            }
        }

        public async Task<RefreshTokenVO> RefreshTokenUser(RefreshTokenVO tokenVO)
        {
            RefreshTokenVO response = new RefreshTokenVO();
            try
            {
                TokenValidationParameters validationToken = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                    ValidateLifetime = false
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                ClaimsPrincipal principal =
                    tokenHandler.ValidateToken(tokenVO.Token, validationToken, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512))
                    throw new SecurityTokenException("Invalid token");

                string username = principal.Identity.Name;
                ApplicationUser userSave = await _userRepo.FindByEmailAsync(username);
                if (userSave == null)
                    return response;

                string refreshToken = await _userRepo.GetTokenAuthenticationAsync(userSave);
                if (refreshToken != tokenVO.RefreshToken)
                    return response;

                await _userRepo.UpdateSecurityStampUserAsync(userSave);

                SymmetricSecurityKey authSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value));

                SigningCredentials credentials =
                    new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512);

                JwtSecurityToken newJwtToken = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: principal.Claims,
                    signingCredentials: credentials
                );

                Result isGenerateRefreshToken = await _userRepo.GenerateRefreshTokenUserAsync(userSave);
                if (isGenerateRefreshToken.IsSuccess)
                {
                    response.RefreshToken = isGenerateRefreshToken.Successes.FirstOrDefault().Message;
                    response.Token = new JwtSecurityTokenHandler().WriteToken(newJwtToken);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorRefreshToken, this.GetType().ToString());
                return response;
            }
        }

        private async Task<TokenUserVO> CreateTokenJwt(ApplicationUser user, List<string> rolesUser)
        {
            try
            {
                List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("username", user.UserName),
                };

                foreach (string role in rolesUser)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                SymmetricSecurityKey authSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value));

                SigningCredentials credentials =
                    new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(5),
                    claims: authClaims,
                    signingCredentials: credentials
                );

                Result result = await _userRepo.GenerateRefreshTokenUserAsync(user);

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                TokenUserVO tokenVO = new TokenUserVO
                {
                    Value = tokenString,
                    RefreshToken = (result.IsSuccess) ? result.Successes.FirstOrDefault().Message : null,
                    Expiration = token.ValidTo
                };

                return tokenVO;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorGenerateTokenJwt + user.Name, this.GetType().ToString());

                return null;
            }
        }

        private async Task<Result> SendEmailConfirmationAccount(ApplicationUser user, HttpRequest request)
        {
            try
            {
                string code = await _userRepo.GenerateTokenConfirmationEmailUserAsync(user);
                string encodedCode = HttpUtility.UrlEncode(code);

                Result response = await _sendMailService.SendMailUserConfirmation(new EmailDataVO
                {
                    UserId = user.Id,
                    Path = string.Concat(request.Scheme, "://",
                                         request.Host.ToUriComponent(),
                                         "/Account/ConfirmAccount?UserId=", user.Id,
                                         "&Code=", encodedCode),
                    Email = user.Email,
                    Link = string.Concat(_configuration["SSLURL"] + _configuration["HostedURL"], "/ConfirmEmail/", user.Id, "/", code.Replace("//", "TOKEN2F"), code.Replace("+", "PLUS2B"))
                });

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorSendEmail, this.GetType().ToString());
                return Result.Fail(ConstantsMessageUsers.ErrorSendEmail);
            }
        }
    }
}