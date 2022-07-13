using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bakehouse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IAccountService _accService;

        public AccountController(ILogService logService, IAccountService accService)
        {
            _logService = logService;
            _accService = accService;
        }

        /// <summary>
        /// Login - Método que faz a autenticação do usuário no sistema, enviar dados no body
        /// </summary>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                TokenUserVO result = await _accService.LoginUser(model);
                if (string.IsNullOrEmpty(result.Error))
                {
                    return StatusCode(StatusCodes.Status202Accepted, new
                    {
                        token = result.Value,
                        refreshToken = result.RefreshToken,
                        expiration = result.Expiration,
                        Accepted = true,
                        User = result.User
                    });
                }

                response.Success = false;
                response.Message = result.Error;
                return StatusCode(StatusCodes.Status401Unauthorized, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorLogInto, this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorLogInto;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Refresh - Método que faz a atualização do token jwt do usuário, enviar dados no body
        /// </summary>
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                RefreshTokenVO result = await _accService.RefreshTokenUser(model);
                if (string.IsNullOrEmpty(result.Token))
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUsers.ErrorRefreshToken;

                    return StatusCode(StatusCodes.Status401Unauthorized, response);
                }

                return StatusCode(StatusCodes.Status202Accepted, new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken
                });
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorRefreshToken, this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorRefreshToken;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// ConfirmAccount - Método que faz a confirmação da conta do usuário com base no código enviado por email, informar
        ///     parametros na query ?UserId=int e ?Code=string
        /// </summary>
        [HttpGet]
        [Route("ConfirmAccount")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] ConfirmAccountVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Result result = await _accService.ConfirmAccountUser(model);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault().Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessConfirmAccountEmail;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorErrorConfirmEmail, this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorErrorConfirmEmail;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// ForgotPassword - Método que faz a requisição para recuperação de senha, informar dados no body
        /// </summary>
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Result result = await _accService.ForgotPasswordSendMail(model);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault().Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessSendMailResetPassword;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.FailedResetPassword, this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.FailedResetPassword;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// ResetPassword - Método que reseta a senha do usuário, informar dado pelo body
        /// </summary>
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Result result = await _accService.ResetPasswordUser(model);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUsers.FailedResetPassword;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessResetPassword;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.FailedResetPassword, this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.FailedResetPassword;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// ResetPasswordSignIn - Método que reseta a senha do usuário logado atualmente
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("ResetPasswordSignIn")]
        public async Task<IActionResult> ResetPasswordSignIn([FromBody] ResetPasswordSignInVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idUser is null || string.IsNullOrEmpty(idUser.Value))
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUsers.ErrorUserNotFound;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                Result result = await _accService.ResetPasswordSignInUser(idUser.Value, model.NewPassword);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault().Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessResetPassword;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorResetPassword, this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorResetPassword;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}