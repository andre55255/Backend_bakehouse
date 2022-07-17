using Bakehouse.Communication.ViewObjects.Category;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface ICategoryService
    {
        public Task<Result> CreateAsync(CategoryVO category);
        public Task<Result> DeleteAsync(int? idCategory);
        public Task<Result> UpdateAsync(CategoryVO category);
        public Task<List<CategoryVO>> FindAllAsync();
        public Task<CategoryVO> FindByIdAsync(int? id);
    }
}