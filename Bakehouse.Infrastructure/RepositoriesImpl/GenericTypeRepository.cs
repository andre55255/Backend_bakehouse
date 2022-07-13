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
    public class GenericTypeRepository : IGenericTypeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public GenericTypeRepository(ApplicationDbContext db, ILogService logService, IMapper mapper)
        {
            _db = db;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                GenericType genericType = await FindByIdAsync(id);

                genericType.DisabledAt = DateTime.Now;
                genericType.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessagesGenericType.ErrorGenericTypeDelete, this.GetType().ToString());

                return Result.Fail(ConstantsMessagesGenericType.ErrorGenericTypeDelete);
            }
        }

        public async Task<List<GenericType>> FindAllAsync()
        {
            try
            {
                List<GenericType> list = await _db.GenericTypes
                                                  .Where(x => x.DisabledAt == null)
                                                  .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessagesGenericType.ErrorGenericTypeFindAll, this.GetType().ToString());

                return null;
            }
        }

        public async Task<GenericType> FindByIdAsync(int id)
        {
            try
            {
                GenericType genericType =
                    await _db.GenericTypes
                              .Where(x => x.DisabledAt == null && x.Id == id)
                              .FirstOrDefaultAsync();

                return genericType;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessagesGenericType.ErrorGenericTypeFindById, this.GetType().ToString());

                return null;
            }
        }

        public async Task<List<GenericType>> FindByTokenAsync(string token)
        {
            try
            {
                List<GenericType> genericTypes = await _db.GenericTypes
                                                          .Where(x => x.DisabledAt == null &&
                                                                      x.Token == token)
                                                          .ToListAsync();

                return genericTypes;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessagesGenericType.ErrorGenericTypeFindByToken, this.GetType().ToString());

                return null;
            }
        }

        public async Task<Result> InsertAsync(GenericType genericType)
        {
            try
            {
                genericType.Id = 0;
                genericType.CreatedAt = DateTime.Now;
                genericType.UpdatedAt = DateTime.Now;

                _db.GenericTypes.Add(genericType);
                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(genericType.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessagesGenericType.ErrorGenericTypeAdd, this.GetType().ToString());
                
                return Result.Fail(ConstantsMessagesGenericType.ErrorGenericTypeAdd);
            }
        }

        public async Task<Result> UpdateAsync(GenericType genericType)
        {
            try
            {
                GenericType save = await FindByIdAsync(genericType.Id);

                genericType.CreatedAt = save.CreatedAt;
                genericType.UpdatedAt = DateTime.Now;
                _mapper.Map(genericType, save);

                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(genericType.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesGenericType.ErrorGenericTypeUpdate, this.GetType().ToString());
                
                return Result.Fail(ConstantsMessagesGenericType.ErrorGenericTypeUpdate);
            }
        }
    }
}