using AutoMapper;
using Bakehouse.Communication.ViewObjects.UnitOfMeasurement;
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
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly IUnitOfMeasurementRepository _unitOfMeasRepo;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public UnitOfMeasurementService(IUnitOfMeasurementRepository unitOfMeasRepo, ILogService logService, IMapper mapper)
        {
            _unitOfMeasRepo = unitOfMeasRepo;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<Result> CreateAsync(UnitOfMeasurementVO unitOfMeas)
        {
            try
            {
                unitOfMeas.Description = unitOfMeas.Description.ToUpper();

                UnitOfMeasurement unitOfMeasEntity = _mapper.Map<UnitOfMeasurement>(unitOfMeas);

                Result result = await _unitOfMeasRepo.InsertAsync(unitOfMeasEntity);
                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorInsert,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorInsert);
            }
        }

        public async Task<Result> DeleteAsync(int? idUnitOfMeas)
        {
            try
            {
                if (!idUnitOfMeas.HasValue)
                    return Result.Fail(ConstantsMessageRequest.ErrorParamNotFound);

                UnitOfMeasurement unitOfMeasExist = await _unitOfMeasRepo.FindByIdAsync(idUnitOfMeas.Value);
                if (unitOfMeasExist is null)
                    return Result.Fail(ConstantsMessageCategory.ErrorNotFound);

                Result existProductWithUnitOfMeas = 
                    await _unitOfMeasRepo.ExistProductWithUnitOfMeasurementByIdAsync(idUnitOfMeas.Value);
                
                if (existProductWithUnitOfMeas.IsFailed)
                    return existProductWithUnitOfMeas;

                Result resultDeleted = await _unitOfMeasRepo.DeleteAsync(idUnitOfMeas.Value);
                return resultDeleted;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorDelete,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorDelete);
            }
        }

        public async Task<List<UnitOfMeasurementVO>> FindAllAsync()
        {
            try
            {
                List<UnitOfMeasurement> catSaves = await _unitOfMeasRepo.FindAllAsync();
                if (catSaves is null)
                    return null;

                List<UnitOfMeasurementVO> response = _mapper.Map<List<UnitOfMeasurementVO>>(catSaves);
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorFindAll,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<UnitOfMeasurementVO> FindByIdAsync(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return null;

                UnitOfMeasurement unitOfMeasSave = await _unitOfMeasRepo.FindByIdAsync(id.Value);
                if (unitOfMeasSave is null)
                    return null;

                UnitOfMeasurementVO response = _mapper.Map<UnitOfMeasurementVO>(unitOfMeasSave);
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorFindAll,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Result> UpdateAsync(UnitOfMeasurementVO unitOfMeas)
        {
            try
            {
                UnitOfMeasurement unitOfMeasExist = await _unitOfMeasRepo.FindByIdAsync(unitOfMeas.Id);
                if (unitOfMeasExist is null)
                    return Result.Fail(ConstantsMessageCategory.ErrorNotFound);

                unitOfMeas.Description = unitOfMeas.Description.ToUpper();
                UnitOfMeasurement unitOfMeasEntity = _mapper.Map<UnitOfMeasurement>(unitOfMeas);

                Result result = await _unitOfMeasRepo.UpdateAsync(unitOfMeasEntity);
                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorUpdate,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorUpdate);
            }
        }
    }
}