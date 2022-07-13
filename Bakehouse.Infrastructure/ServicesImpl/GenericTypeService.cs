using AutoMapper;
using Bakehouse.Communication.ViewObjects;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class GenericTypeService : IGenericTypeService
    {
        private readonly ILogService _logService;
        private readonly IGenericTypeRepository _genRepo;
        private readonly IMapper _mapper;

        public GenericTypeService(ILogService logService, IGenericTypeRepository genRepo, IMapper mapper)
        {
            _logService = logService;
            _genRepo = genRepo;
            _mapper = mapper;
        }

        public async Task<Result> CreateAsync(GenericTypeVO genericTypeVO)
        {
            try
            {
                GenericType genericType = _mapper.Map<GenericType>(genericTypeVO);

                Result result = await _genRepo.InsertAsync(genericType);

                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeAdd, this.GetType().ToString());
                
                return Result.Fail(ConstantsMessagesGenericType.ErrorGenericTypeAdd);
            }
        }

        public async Task<Result> DeleteAsync(int idGeneric)
        {
            try
            {
                Result result = await _genRepo.DeleteAsync(idGeneric);
                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeDelete, this.GetType().ToString());
                
                return Result.Fail(ConstantsMessagesGenericType.ErrorGenericTypeDelete);
            }
        }

        public async Task<List<GenericTypeVO>> FindAllAsync()
        {
            try
            {
                List<GenericType> list = await _genRepo.FindAllAsync();

                List<GenericTypeVO> response = _mapper.Map<List<GenericTypeVO>>(list);

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeFindAll, this.GetType().ToString());
                
                return null;
            }
        }

        public async Task<GenericTypeVO> FindByIdAsync(int id)
        {
            try
            {
                GenericType genericType = await _genRepo.FindByIdAsync(id);
                GenericTypeVO genericTypeVO = _mapper.Map<GenericTypeVO>(genericType);
                return genericTypeVO;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessagesGenericType.ErrorGenericTypeFindById, this.GetType().ToString());
                
                return null;
            }
        }

        public async Task<Result> UpdateAsync(GenericTypeVO genericTypeVO)
        {
            try
            {
                GenericType genericType = _mapper.Map<GenericType>(genericTypeVO);

                Result result = await _genRepo.UpdateAsync(genericType);

                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeAdd, this.GetType().ToString());
                
                return Result.Fail(ConstantsMessagesGenericType.ErrorGenericTypeUpdate);
            }
        }
    }
}