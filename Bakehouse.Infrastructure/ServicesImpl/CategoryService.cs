using AutoMapper;
using Bakehouse.Communication.ViewObjects.Category;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _catRepo;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository catRepo, ILogService logService, IMapper mapper)
        {
            _catRepo = catRepo;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<Result> CreateAsync(CategoryVO category)
        {
            try
            {
                category.Description = category.Description.ToUpper();

                Category categoryEntity = _mapper.Map<Category>(category);

                Result result = await _catRepo.InsertAsync(categoryEntity);
                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorInsert,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageCategory.ErrorInsert);
            }
        }

        public async Task<Result> DeleteAsync(int? idCategory)
        {
            try
            {
                if (!idCategory.HasValue)
                    return Result.Fail(ConstantsMessageRequest.ErrorParamNotFound);

                Category catExist = await _catRepo.FindByIdAsync(idCategory.Value);
                if (catExist is null)
                    return Result.Fail(ConstantsMessageCategory.ErrorNotFound);

                Result existProductWithCategory = await _catRepo.ExistProductWithCategoryByIdAsync(idCategory.Value);
                if (existProductWithCategory.IsFailed)
                    return existProductWithCategory;

                Result resultDeleted = await _catRepo.DeleteAsync(idCategory.Value);
                return resultDeleted;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorDelete,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageCategory.ErrorDelete);
            }
        }

        public async Task<List<CategoryVO>> FindAllAsync()
        {
            try
            {
                List<Category> catSaves = await _catRepo.FindAllAsync();
                if (catSaves is null)
                    return null;

                List<CategoryVO> response = _mapper.Map<List<CategoryVO>>(catSaves);
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorFindAll,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<CategoryVO> FindByIdAsync(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return null;

                Category catSave = await _catRepo.FindByIdAsync(id.Value);
                if (catSave is null)
                    return null;

                CategoryVO response = _mapper.Map<CategoryVO>(catSave);
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageCategory.ErrorFindAll,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Result> UpdateAsync(CategoryVO category)
        {
            try
            {
                Category catExist = await _catRepo.FindByIdAsync(category.Id);
                if (catExist is null)
                    return Result.Fail(ConstantsMessageCategory.ErrorNotFound);

                category.Description = category.Description.ToUpper();
                Category categoryEntity = _mapper.Map<Category>(category);

                Category catNameExist = await _catRepo.FindByDescriptionAsync(category.Description);
                if (catNameExist is not null)
                    return Result.Fail(ConstantsMessageCategory.ErrorNameExists);

                Result result = await _catRepo.UpdateAsync(categoryEntity);
                return result;
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