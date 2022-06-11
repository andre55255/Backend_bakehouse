﻿using AutoMapper;
using Bakehouse.Communication.ViewObjects.Configuration;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configRepo;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public ConfigurationService(IConfigurationRepository configRepo, ILogService logService, IMapper mapper)
        {
            _configRepo = configRepo;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<Result> CreateAsync(ConfigurationVO config)
        {
            try
            {
                config.Id = 0;
                Configuration configSave = _mapper.Map<Configuration>(config);

                Result result = await _configRepo.InsertAsync(configSave);
                if (result.IsSuccess)
                    return Result.Ok();

                return Result.Fail(result.Errors.FirstOrDefault().Message);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorInsert, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorInsert);
            }
        }

        public async Task<Result> DeleteAsync(string token)
        {
            try
            {
                Result result = await _configRepo.DeleteByTokenAsync(token);

                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorDelete, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorDelete);
            }
        }

        public async Task<List<ConfigurationVO>> FindAllAsync()
        {
            try
            {
                List<Configuration> configsSave = await _configRepo.FindAllAsync();
                if (configsSave == null)
                    return null;

                List<ConfigurationVO> response = _mapper.Map<List<ConfigurationVO>>(configsSave);
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorFindAll, this.GetType().ToString());
                return null;
            }
        }

        public async Task<ConfigurationVO> FindByIdAsync(int id)
        {
            try
            {
                Configuration save = await _configRepo.FindByIdAsync(id);
                if (save == null)
                    return null;

                ConfigurationVO configReturnVO = _mapper.Map<ConfigurationVO>(save);
                return configReturnVO;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorFindById,
                    this.GetType().ToString());
                return null;
            }
        }

        public async Task<ConfigurationVO> FindByTokenAsync(string token)
        {
            try
            {
                Configuration save = await _configRepo.FindByTokenAsync(token);
                if (save == null)
                    return null;

                ConfigurationVO configReturnVO = _mapper.Map<ConfigurationVO>(save);
                return configReturnVO;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorFindAll, this.GetType().ToString());
                return null;
            }
        }

        public async Task<Result> UpdateByTokenAsync(ConfigurationVO config)
        {
            try
            {
                Configuration configEntity = _mapper.Map<Configuration>(config);
                Result result = await _configRepo.UpdateAsync(configEntity);

                if (result.IsSuccess)
                    return Result.Ok();

                return Result.Fail(result.Errors.FirstOrDefault().Message);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageConfiguration.ErrorUpdate, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorUpdate);
            }
        }
    }
}