using Bakehouse.Core.Entities;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.RepositoriesInterface
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> FindAllAsync();
        public Task<Category> FindByIdAsync(int id);
        public Task<Result> InsertAsync(Category category);
        public Task<Result> UpdateAsync(Category category);
        public Task<Result> DeleteAsync(int id);
        public Task<Result> ExistProductWithCategoryByIdAsync(int id);
    }
}