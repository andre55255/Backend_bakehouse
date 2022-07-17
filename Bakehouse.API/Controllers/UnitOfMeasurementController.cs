using Bakehouse.Communication.ViewObjects.UnitOfMeasurement;
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
    public class UnitOfMeasurementController : ControllerBase
    {
        private readonly IUnitOfMeasurementService _unitOfMeasService;
        private readonly ILogService _logService;

        public UnitOfMeasurementController(IUnitOfMeasurementService unitOfMeasService, ILogService logService)
        {
            _unitOfMeasService = unitOfMeasService;
            _logService = logService;
        }

        /// <summary>
        /// Save - Método que cria/edita uma unidade de medida no banco de dados, enviar dados no body (Informar id=-1 para cadastro e o id da unidade de medida para editar)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> SaveUnitOfMeasurement([FromBody] UnitOfMeasurementVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (model.Id == -1)
                {
                    Result result = await _unitOfMeasService.CreateAsync(model);

                    if (result.IsFailed)
                    {
                        response.Success = false;
                        response.Message = result.Errors.FirstOrDefault()?.Message;

                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    int idItemAdd = int.Parse(result.Successes.FirstOrDefault().Message);
                    response.Object = await _unitOfMeasService.FindByIdAsync(idItemAdd);

                    response.Message = ConstantsMessageUnitOfMeasurement.SuccessInsert;
                    response.Success = true;

                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    UnitOfMeasurementVO saveDB =
                        await _unitOfMeasService.FindByIdAsync(model.Id);

                    if (saveDB is null)
                    {
                        response.Success = false;
                        response.Message = ConstantsMessageUnitOfMeasurement.ErrorNotFound;

                        return StatusCode(StatusCodes.Status404NotFound, response);
                    }

                    Result result = await _unitOfMeasService.UpdateAsync(model);
                    if (result.IsFailed)
                    {
                        response.Success = false;
                        response.Message = result.Errors.FirstOrDefault()?.Message;

                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }

                    response.Success = true;
                    response.Message = ConstantsMessageUnitOfMeasurement.SuccessUpdate;
                    response.Object = saveDB;

                    return StatusCode(StatusCodes.Status200OK, response);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorSave,
                    this.GetType().ToString());

                response.Message = ConstantsMessageUnitOfMeasurement.ErrorSave;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetById - Método que retorna uma unidade de medida por id, informar parametro na query ?id=int
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetByIdUnitOfMeasurement([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                UnitOfMeasurementVO saveDB = await _unitOfMeasService.FindByIdAsync(id);
                if (saveDB == null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageUnitOfMeasurement.ErrorNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = saveDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUnitOfMeasurement.ErrorFindById, this.GetType().ToString());

                response.Message = ConstantsMessageUnitOfMeasurement.ErrorFindById;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetAll - Método que retorna todas as unidades de medida do banco de dados, sem parametros
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUnitOfMeasurement()
        {
            APIResponse response = new APIResponse();
            try
            {
                List<UnitOfMeasurementVO> listDB = await _unitOfMeasService.FindAllAsync();

                if (listDB == null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageUnitOfMeasurement.ErrorTypeDbEmpty;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                response.Success = true;
                response.Object = listDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUnitOfMeasurement.ErrorFindAll, this.GetType().ToString());

                response.Message = ConstantsMessageUnitOfMeasurement.ErrorFindAll;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Delete - Método que desabilita uma unidade de medida no banco de dados, informar parametro na query ?id=int
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUnitOfMeasurement([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                UnitOfMeasurementVO saveDB = await _unitOfMeasService.FindByIdAsync(id);
                if (saveDB == null)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUnitOfMeasurement.ErrorNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                Result result = await _unitOfMeasService.DeleteAsync(id);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageUnitOfMeasurement.ErrorNotFound;

                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageUnitOfMeasurement.SuccessDelete;
                response.Object = saveDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUnitOfMeasurement.ErrorDelete, this.GetType().ToString());

                response.Message = ConstantsMessageUnitOfMeasurement.ErrorDelete;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
