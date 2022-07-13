using Bakehouse.Communication.ViewObjects.Configuration;
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
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configService;
        private readonly ILogService _logService;

        public ConfigurationController(IConfigurationService configService, ILogService logService)
        {
            _configService = configService;
            _logService = logService;
        }

        /// <summary>
        /// Save - Método para criar/editar configuração no banco, passar dados no body. Id=-1 para inserir e para editar passar o id da config 
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> SaveConfiguration([FromBody] ConfigurationVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                Result result = null;
                if (model.Id == -1)
                {
                    result = await _configService.CreateAsync(model);
                    if (result.IsFailed)
                    {
                        response.Success = false;
                        response.Message = result.Errors.FirstOrDefault().Message;

                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    int idCreated = result.Successes.FirstOrDefault() is not null ?
                             int.Parse(result.Successes.FirstOrDefault().Message) :
                             0;

                    ConfigurationVO configSave = await _configService.FindByIdAsync(idCreated);

                    response.Success = true;
                    response.Message = ConstantsMessageConfiguration.SuccessInsert;
                    response.Object = configSave;

                    return StatusCode(StatusCodes.Status201Created, response);
                }
                result = await _configService.UpdateByTokenAsync(model);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = result.Errors.FirstOrDefault().Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                int idEdit = result.Successes.FirstOrDefault() is not null ?
                             int.Parse(result.Successes.FirstOrDefault().Message) :
                             0;

                ConfigurationVO configEdit = await _configService.FindByIdAsync(idEdit);

                response.Success = true;
                response.Message = ConstantsMessageConfiguration.SuccessUpdate;
                response.Object = configEdit;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorSave,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageConfiguration.ErrorSave;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetAll - Método para listar todas as configurações no banco de dados, sem parâmetros
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllConfiguration()
        {
            APIResponse response = new APIResponse();
            try
            {
                List<ConfigurationVO> configs = await _configService.FindAllAsync();
                if (configs is null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageConfiguration.ErrorTypeDbEmpty;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = configs;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorFindAll,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageConfiguration.ErrorFindAll;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetById - Método para buscar uma configuração por id, passar parâmetro pela query, ?id=int
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetByIdConfiguration([FromQuery] int? id)
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
                ConfigurationVO config = await _configService.FindByIdAsync(id.Value);
                if (config is null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageConfiguration.ErrorTypeDbEmpty;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = config;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorFindById,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageConfiguration.ErrorFindById;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetById - Método para buscar uma configuração por id, passar parâmetro pela query, ?token=string
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetByToken")]
        public async Task<IActionResult> GetByTokenConfiguration([FromQuery] string token)
        {
            APIResponse response = new APIResponse();
            try
            {
                ConfigurationVO config = await _configService.FindByTokenAsync(token);
                if (config is null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageConfiguration.ErrorTypeDbEmpty;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = config;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorFindByToken,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageConfiguration.ErrorFindByToken;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Delete - Método para deletar uma configuração por id ou por token, informar parâmetros pela query, ?id=int ou ?token=string. Caso informe os dois será excluído por id
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfiguration([FromQuery] int? id, string token)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (!id.HasValue && string.IsNullOrEmpty(token))
                {
                    response.Success = false;
                    response.Message = ConstantsMessageRequest.ErrorParamNotFound;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                if (id.HasValue)
                {
                    Result resultDeleteById = await _configService.DeleteAsync(id.Value, null);
                    if (resultDeleteById.IsFailed)
                    {
                        response.Success = false;
                        response.Message = resultDeleteById.Errors.FirstOrDefault().Message;

                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }
                    response.Success = true;
                    response.Message = ConstantsMessageConfiguration.SuccessDelete;

                    return StatusCode(StatusCodes.Status200OK, response);
                }
                Result resultDeleteByToken = await _configService.DeleteAsync(null, token);
                if (resultDeleteByToken.IsFailed)
                {
                    response.Success = false;
                    response.Message = resultDeleteByToken.Errors.FirstOrDefault().Message;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageConfiguration.SuccessDelete;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorDelete,
                    this.GetType().ToString());

                response.Success = false;
                response.Message = ConstantsMessageConfiguration.ErrorDelete;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}