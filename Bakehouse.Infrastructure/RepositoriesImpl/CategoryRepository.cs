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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogService _logService;

        public CategoryRepository(ApplicationDbContext db, ILogService logService)
        {
            _db = db;
            _logService = logService;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                Category save = await _db.Categories
                                         .Where(x => x.Id == id && x.DisabledAt == null)
                                         .FirstOrDefaultAsync();

                save.DisabledAt = DateTime.Now;
                save.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();
                return Result.Ok().WithSuccess(save.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorDelete,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageCategory.ErrorDelete);
            }
        }

        public async Task<Result> ExistProductWithCategoryByIdAsync(int id)
        {
            try
            {
                List<Product> products = await _db.Products
                                                  .Where(x => x.DisabledAt == null &&
                                                              x.CategoryId == id)
                                                  .ToListAsync();

                if (products is null || products.Count() <= 0)
                    return Result.Ok();

                return Result.Fail(ConstantsMessageCategory.ErrorExistProductWithCategory);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorVerifyExistProductWithCategory,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageCategory.ErrorVerifyExistProductWithCategory);
            }
        }

        public async Task<List<Category>> FindAllAsync()
        {
            try
            {
                List<Category> categories = await _db.Categories
                                                     .Where(x => x.DisabledAt == null)
                                                     .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorFindAll,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Category> FindByDescriptionAsync(string description)
        {
            try
            {
                Category category = await _db.Categories
                                             .Where(x => x.Description == description && 
                                                         x.DisabledAt == null)
                                             .FirstOrDefaultAsync();

                return category;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorFindByName,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            try
            {
                Category category = await _db.Categories
                                             .Where(x => x.Id == id && x.DisabledAt == null)
                                             .FirstOrDefaultAsync();

                return category;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorFindById,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Result> InsertAsync(Category category)
        {
            try
            {
                Category catWithNameExist = await _db.Categories
                                                     .Where(x => x.Description == category.Description &&
                                                                 x.DisabledAt == null)
                                                     .FirstOrDefaultAsync();

                if (catWithNameExist is not null)
                    return Result.Fail(ConstantsMessageUnitOfMeasurement.ErrorNameExists);

                category.Id = 0;
                category.CreatedAt = DateTime.Now;
                category.UpdatedAt = DateTime.Now;

                _db.Categories.Add(category);
                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(category.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorSave,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageCategory.ErrorSave);
            }
        }

        public async Task<Result> UpdateAsync(Category category)
        {
            try
            {
                Category save = await FindByIdAsync(category.Id);

                save.Description = category.Description;
                save.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Result.Ok().WithSuccess(category.Id.ToString());
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorUpdate,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageCategory.ErrorUpdate);
            }
        }
    }
}