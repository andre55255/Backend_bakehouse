using Bakehouse.Communication.ViewObjects.Configuration;
using Bakehouse.Communication.ViewObjects.Utils;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IConfigurationService
    {
        public Task<Result> CreateAsync(ConfigurationVO config);
        public Task<Result> DeleteAsync(string token);
        public Task<Result> UpdateByTokenAsync(ConfigurationVO config);
        public Task<List<ConfigurationVO>> FindAllAsync();
        public Task<ConfigurationVO> FindByTokenAsync(string token);
        public Task<ConfigurationVO> FindByIdAsync(int id);
    }
}