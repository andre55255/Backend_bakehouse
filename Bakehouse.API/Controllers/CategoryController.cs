using Bakehouse.Communication.ViewObjects.Category;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _catService;
        private readonly ILogService _logService;

        public CategoryController(ICategoryService catService, ILogService logService)
        {
            _catService = catService;
            _logService = logService;
        }

        /// <summary>
        /// Save - Método que cria/edita uma categoria no banco de dados, enviar dados no body (Informar id=-1 para cadastro e o id da categoria para editar)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> SaveCategory([FromBody] CategoryVO model)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (model.Id == -1)
                {
                    Result result = await _catService.CreateAsync(model);

                    if (result.IsFailed)
                    {
                        response.Message = result.Errors.FirstOrDefault()?.Message;
                        response.Success = false;

                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    int idItemAdd = int.Parse(result.Successes.FirstOrDefault().Message);
                    response.Object = await _catService.FindByIdAsync(idItemAdd);

                    response.Message = ConstantsMessageCategory.SuccessInsert;
                    response.Success = true;

                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    CategoryVO saveDB =
                        await _catService.FindByIdAsync(model.Id);

                    if (saveDB is null)
                    {
                        response.Success = false;
                        response.Message = ConstantsMessageCategory.ErrorNotFound;

                        return StatusCode(StatusCodes.Status404NotFound, response);
                    }

                    Result result = await _catService.UpdateAsync(model);
                    if (result.IsFailed)
                    {
                        response.Success = false;
                        response.Message = result.Errors.FirstOrDefault()?.Message;

                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }

                    response.Success = true;
                    response.Message = ConstantsMessageCategory.SuccessUpdate;
                    response.Object = saveDB;

                    return StatusCode(StatusCodes.Status200OK, response);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorSave,
                    this.GetType().ToString());

                response.Message = ConstantsMessageCategory.ErrorSave;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetById - Método que retorna uma categoria por id, informar parametro na query ?id=int
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetByIdCategory([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                CategoryVO saveDB = await _catService.FindByIdAsync(id);
                if (saveDB == null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageCategory.ErrorNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                response.Success = true;
                response.Object = saveDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageCategory.ErrorFindById, this.GetType().ToString());

                response.Message = ConstantsMessageCategory.ErrorFindById;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// GetAll - Método que retorna todas categorias do banco de dados, sem parametros
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllCategory()
        {
            APIResponse response = new APIResponse();
            try
            {
                List<CategoryVO> listDB = await _catService.FindAllAsync();

                if (listDB == null)
                {
                    response.Success = true;
                    response.Message = ConstantsMessageCategory.ErrorTypeDbEmpty;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                response.Success = true;
                response.Object = listDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageCategory.ErrorFindAll, this.GetType().ToString());

                response.Message = ConstantsMessageCategory.ErrorFindAll;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Delete - Método que desabilita uma categoria no banco de dados, informar parametro na query ?id=int
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteCategory([FromQuery] int? id)
        {
            APIResponse response = new APIResponse();
            try
            {
                CategoryVO saveDB = await _catService.FindByIdAsync(id);
                if (saveDB == null)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageCategory.ErrorNotFound;

                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
                Result result = await _catService.DeleteAsync(id);
                if (result.IsFailed)
                {
                    response.Success = false;
                    response.Message = ConstantsMessageCategory.ErrorNotFound;

                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                response.Success = true;
                response.Message = ConstantsMessageCategory.SuccessDelete;
                response.Object = saveDB;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageCategory.ErrorDelete, this.GetType().ToString());

                response.Message = ConstantsMessageCategory.ErrorDelete;
                response.Success = false;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
