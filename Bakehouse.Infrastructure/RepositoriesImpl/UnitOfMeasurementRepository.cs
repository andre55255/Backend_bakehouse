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
    public class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogService _logService;

        public UnitOfMeasurementRepository(ApplicationDbContext db, ILogService logService)
        {
            _db = db;
            _logService = logService;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                UnitOfMeasurement save = await _db.UnitOfMeasurements
                                                  .Where(x => x.Id == id && x.DisabledAt == null)
                                                  .FirstOrDefaultAsync();

                save.DisabledAt = DateTime.Now;
                save.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();
                return Result.Ok().WithSuccess(save.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorDelete,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorDelete);
            }
        }

        public async Task<Result> ExistProductWithUnitOfMeasurementByIdAsync(int id)
        {
            try
            {
                List<Product> products = await _db.Products
                                                  .Where(x => x.DisabledAt == null &&
                                                              x.UnitOfMeasurementId == id)
                                                  .ToListAsync();

                if (products is null || products.Count() <= 0)
                    return Result.Ok();

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorExistProductWithCategory);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorVerifyExistProductWithCategory,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorVerifyExistProductWithCategory);
            }
        }

        public async Task<List<UnitOfMeasurement>> FindAllAsync()
        {
            try
            {
                List<UnitOfMeasurement> unitOfMeasurements = await _db.UnitOfMeasurements
                                                                      .Where(x => x.DisabledAt == null)
                                                                      .ToListAsync();

                return unitOfMeasurements;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorFindAll,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<UnitOfMeasurement> FindByDescriptionAsync(string description)
        {
            try
            {
                UnitOfMeasurement unitOfMeasurement = await _db.UnitOfMeasurements
                                                               .Where(x => x.Description == description && 
                                                                           x.DisabledAt == null)
                                                               .FirstOrDefaultAsync();

                return unitOfMeasurement;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorFindByName,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<UnitOfMeasurement> FindByIdAsync(int id)
        {
            try
            {
                UnitOfMeasurement unitOfMeasurement = await _db.UnitOfMeasurements
                                                               .Where(x => x.Id == id && x.DisabledAt == null)
                                                               .FirstOrDefaultAsync();

                return unitOfMeasurement;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorFindById,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Result> InsertAsync(UnitOfMeasurement unitOfMeas)
        {
            try
            {
                UnitOfMeasurement unitOfMeasWithNameExist = 
                    await _db.UnitOfMeasurements
                             .Where(x => x.Description == unitOfMeas.Description &&
                                         x.DisabledAt == null)
                             .FirstOrDefaultAsync();

                if (unitOfMeasWithNameExist is not null)
                    return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorNameExists);

                unitOfMeas.Id = 0;
                unitOfMeas.CreatedAt = DateTime.Now;
                unitOfMeas.UpdatedAt = DateTime.Now;

                _db.UnitOfMeasurements.Add(unitOfMeas);
                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(unitOfMeas.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageUnitOfMeasurement.ErrorInsert,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorInsert);
            }
        }

        public async Task<Result> UpdateAsync(UnitOfMeasurement unitOfMeas)
        {
            try
            {
                UnitOfMeasurement save = await FindByIdAsync(unitOfMeas.Id);

                save.Description = unitOfMeas.Description;
                save.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(unitOfMeas.Id.ToString());
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