using Bakehouse.Core.Entities;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.RepositoriesInterface
{
    public interface IConfigurationRepository
    {
        public Task<List<Configuration>> FindAllAsync();
        public Task<Configuration> FindByTokenAsync(string token);
        public Task<Configuration> FindByIdAsync(int id);
        public Task<Result> InsertAsync(Configuration configuration);
        public Task<Result> UpdateAsync(Configuration configuration);
        public Task<Result> DeleteByTokenAsync(string token);
        public Task<Result> DeleteByIdAsync(int id);
    }
}