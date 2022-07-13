using AutoMapper;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using Bakehouse.Infrastructure.Data.Context;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.RepositoriesImpl
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public ConfigurationRepository(ApplicationDbContext db, ILogService logService, IMapper mapper)
        {
            _db = db;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<Result> DeleteByIdAsync(int id)
        {
            try
            {
                Configuration configSave = await FindByIdAsync(id);
                if (configSave == null)
                    return Result.Fail(ConstantsMessageConfiguration.ErrorConfigNotFound);

                configSave.UpdatedAt = DateTime.Now;
                configSave.DisabledAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(configSave.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorDelete, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorDelete);
            }
        }

        public async Task<Result> DeleteByTokenAsync(string token)
        {
            try
            {
                Configuration configSave = await FindByTokenAsync(token);
                if (configSave == null)
                    return Result.Fail(ConstantsMessageConfiguration.ErrorConfigNotFound);

                configSave.UpdatedAt = DateTime.Now;
                configSave.DisabledAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(configSave.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorDelete, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorDelete);
            }
        }

        public async Task<List<Configuration>> FindAllAsync()
        {
            try
            {
                List<Configuration> configurationsSave = await _db.Configurations
                                                                  .Where(x => x.DisabledAt == null)
                                                                  .ToListAsync();

                return configurationsSave;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorFindAll, this.GetType().ToString());
                return null;
            }
        }

        public async Task<Configuration> FindByIdAsync(int id)
        {
            try
            {
                Configuration config = await _db.Configurations
                                                .Where(x => x.Id == id &&
                                                            x.DisabledAt == null)
                                                .FirstOrDefaultAsync();

                return config;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorFindById, this.GetType().ToString());
                return null;
            }
        }

        public async Task<Configuration> FindByTokenAsync(string token)
        {
            try
            {
                Configuration config = await _db.Configurations
                                                .Where(x => x.Token == token &&
                                                            x.DisabledAt == null)
                                                .FirstOrDefaultAsync();

                return config;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorFindByToken, this.GetType().ToString());
                return null;
            }
        }

        public async Task<Result> InsertAsync(Configuration configuration)
        {
            try
            {
                configuration.CreatedAt = DateTime.Now;
                configuration.UpdatedAt = DateTime.Now;

                _db.Configurations.Add(configuration);
                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(configuration.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorInsert, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorInsert);
            }
        }

        public async Task<Result> UpdateAsync(Configuration configuration)
        {
            try
            {
                Configuration save = await FindByTokenAsync(configuration.Token);

                if (save == null)
                    return Result.Fail(ConstantsMessageConfiguration.ErrorConfigNotFound);

                configuration.UpdatedAt = DateTime.Now;
                configuration.CreatedAt = save.CreatedAt;

                _mapper.Map(configuration, save);
                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(configuration.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageConfiguration.ErrorUpdate, this.GetType().ToString());
                return Result.Fail(ConstantsMessageConfiguration.ErrorUpdate);
            }
        }
    }
}