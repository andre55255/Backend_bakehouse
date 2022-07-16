using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakehouse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public UserController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        /// <summary>
        /// Create - Método para criação de usuário, passar dados pelo body (ProfileImage deve ser informada com o file em base64). Informar id=-1
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateUser([FromBody] SaveUserVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Result result = await _userService.CreateAsync(model);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault()?.Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                string idUserCreated = result.Successes.FirstOrDefault()?.Message;
                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessCreated;
                response.Object = idUserCreated is null ?
                                                null :
                                                await _userService.GetByIdAsync(int.Parse(idUserCreated));

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorCreateUser,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorCreateUser;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Update - Método para editar dados de usuário, passar dados pelo body (ProfileImage deve ser informada com o file em base64).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Result result = await _userService.UpdateAsync(model);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault()?.Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                string idUserCreated = result.Successes.FirstOrDefault()?.Message;
                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessCreated;
                response.Object = idUserCreated is null ?
                                                null :
                                                await _userService.GetByIdAsync(int.Parse(idUserCreated));

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorUpdateUser,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorCreateUser;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetById - Método para buscar um usuário por id, passar id pela query ?id=int
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetByIdUser([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (!id.HasValue)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageRequest.ErrorParamNotFound;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                UserVO user = await _userService.GetByIdAsync(id.Value);
                if (user is null)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUsers.ErrorUserNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = user;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorGetById,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorGetById;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetAll - Método para listar usuários, sem parâmetros
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUser()
        {
            APIResponse response = new APIResponse();
            try
            {
                List<UserVO> user = await _userService.GetAllAsync();
                if (user is null)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUsers.ErrorUserNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = user;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorGetAllUsers,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorGetAllUsers;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Delete - Método para desabilitar usuário, passar id do usuário pela query ?id=user
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUser([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (!id.HasValue)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageRequest.ErrorParamNotFound;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                Result result = await _userService.DeleteAsync(id.Value);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault()?.Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageUsers.SuccessDisabled;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorDisableUser,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorDisableUser;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Prepare - Método para preparar tela de usuário. Preparar tela de criação, sem parametros. Preparar tela de edição, parassar id do usuário pela query ?id=int
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Prepare")]
        public async Task<IActionResult> PrepareUser([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                PrepareUserVO prepareVO = new PrepareUserVO();
                List<SelectObjectVO> roles = await _userService.GetRolesAsSelectObject();
                if (roles is null)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUsers.ErrorFindAllRoles;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                prepareVO.Roles = roles;
                if (id.HasValue)
                {
                    UserVO user = await _userService.GetByIdAsync(id.Value);
                    if (user is null)
                    {
                        response.Success = false;
                        response.Message = ConstantsMessageUsers.ErrorUserNotFound;

                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }
                    prepareVO.User = user;
                }

                response.Object = prepareVO;
                response.Success = true;
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUsers.ErrorPrepare,
                   this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageUsers.ErrorPrepare;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
