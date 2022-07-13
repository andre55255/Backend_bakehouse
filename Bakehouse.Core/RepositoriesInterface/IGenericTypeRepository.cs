using Bakehouse.Core.Entities;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.RepositoriesInterface
{
    public interface IGenericTypeRepository
    {
        public Task<List<GenericType>> FindAllAsync();
        public Task<GenericType> FindByIdAsync(int id);
        public Task<Result> InsertAsync(GenericType genericType);
        public Task<Result> UpdateAsync(GenericType genericType);
        public Task<Result> DeleteAsync(int id);
        public Task<List<GenericType>> FindByTokenAsync(string token);
    }
}