using Bakehouse.Communication.ViewObjects.UnitOfMeasurement;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IUnitOfMeasurementService
    {
        public Task<Result> CreateAsync(UnitOfMeasurementVO unitOfMeas);
        public Task<Result> DeleteAsync(int? idUnitOfMeas);
        public Task<Result> UpdateAsync(UnitOfMeasurementVO unitOfMeas);
        public Task<List<UnitOfMeasurementVO>> FindAllAsync();
        public Task<UnitOfMeasurementVO> FindByIdAsync(int? id);
    }
}