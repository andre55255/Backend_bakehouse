using Bakehouse.Communication.ViewObjects;
using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
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
    public class GenericTypeController : ControllerBase
    {
        private readonly IGenericTypeService _genericTypeService;
        private readonly ILogService _logService;

        public GenericTypeController(IGenericTypeService genericTypeService, ILogService logService)
        {
            _genericTypeService = genericTypeService;
            _logService = logService;
        }

        /// <summary>
        /// Save - Método que cria/edita um genericType no banco de dados, enviar dados no body (Informar id=-1 para cadastro e o id do genericType para editar)
        /// </summary>
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> SaveGenericType([FromBody] GenericTypeVO genericTypeVO)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (genericTypeVO.Id == -1)
                {
                    Result result = await _genericTypeService.CreateAsync(genericTypeVO);

                    if (result.IsFailed)
                    {
                        response.Message = result.Errors.FirstOrDefault().Message;
                        response.Success = false;

                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    int idItemAdd = int.Parse(result.Successes.FirstOrDefault().Message);
                    response.Object = await _genericTypeService.FindByIdAsync(idItemAdd);

                    response.Message = ConstantsMessagesGenericType.SuccessGenericTypeCreate;
                    response.Success = true;

                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    GenericTypeVO genericTypeSaveDB =
                        await _genericTypeService.FindByIdAsync(genericTypeVO.Id);

                    if (genericTypeSaveDB is null)
                    {
                        response.Success = false;
                        response.Message = ConstantsMessagesGenericType.ErrorGenericTypeFindById;

                        return StatusCode(StatusCodes.Status404NotFound, response);
                    }

                    Result result = await _genericTypeService.UpdateAsync(genericTypeVO);
                    if (result.IsFailed)
                    {
                        response.Success = false;
                        response.Message = ConstantsMessagesGenericType.ErrorGenericTypeUpdate;

                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }

                    response.Success = true;
                    response.Message = ConstantsMessagesGenericType.SuccessGenericTypeUpdate;
                    response.Object = genericTypeVO;

                    return StatusCode(StatusCodes.Status200OK, response);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessagesGenericType.ErrorGenericTypeSave,
                    this.GetType().ToString());

                response.Message = ConstantsMessagesGenericType.ErrorGenericTypeSave;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetById - Método que retorna um GenericType por id no banco de dados, informar parametro na query ?id=int
        /// </summary>
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetByIdGenericType([FromQuery] int? id)
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
                GenericTypeVO genericTypeVO = await _genericTypeService.FindByIdAsync(id.Value);
                if (genericTypeVO == null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessagesGenericType.ErrorGenericTypeNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = genericTypeVO;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeFindById, this.GetType().ToString());

                response.Message = ConstantsMessagesGenericType.ErrorGenericTypeFindById;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetAll - Método que retorna todos os GenericType do banco de dados, sem parametros
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllGenericType()
        {
            APIResponse response = new APIResponse();
            try
            {
                List<GenericTypeVO> listGenericTypes = await _genericTypeService.FindAllAsync();

                if (listGenericTypes == null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessagesGenericType.ErrorGenericTypeDbEmpty;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                response.Success = true;
                response.Object = listGenericTypes;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeFindAll, this.GetType().ToString());

                response.Message = ConstantsMessagesGenericType.ErrorGenericTypeFindAll;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Delete - Método que desabilita um GenericType no banco de dados, informar parametro na query ?id=int
        /// </summary>
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteGenericType([FromQuery] int? id)
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
                GenericTypeVO genericTypeSaveDB = await _genericTypeService.FindByIdAsync(id.Value);
                if (genericTypeSaveDB == null)
                {
                    response.Success = false;
                    response.Message = ConstantsMessagesGenericType.ErrorGenericTypeFindById;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                Result result = await _genericTypeService.DeleteAsync(id.Value);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = ConstantsMessagesGenericType.ErrorGenericTypeDelete;

                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                response.Success = true;
                response.Message = ConstantsMessagesGenericType.SuccessGenericTypeDelete;
                response.Object = genericTypeSaveDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeDelete, this.GetType().ToString());

                response.Message = ConstantsMessagesGenericType.ErrorGenericTypeDelete;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
