using Bakehouse.Core.Entities;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.RepositoriesInterface
{
    public interface IUnitOfMeasurementRepository
    {
        public Task<List<UnitOfMeasurement>> FindAllAsync();
        public Task<UnitOfMeasurement> FindByIdAsync(int id);
        public Task<Result> InsertAsync(UnitOfMeasurement unitOfMeas);
        public Task<Result> UpdateAsync(UnitOfMeasurement unitOfMeas);
        public Task<Result> DeleteAsync(int id);
        public Task<Result> ExistProductWithUnitOfMeasurementByIdAsync(int id);
    }
}