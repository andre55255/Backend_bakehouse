using Bakehouse.Communication.ViewObjects;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IGenericTypeService
    {
        public Task<Result> CreateAsync(GenericTypeVO genericTypeVO);
        public Task<GenericTypeVO> FindByIdAsync(int id);
        public Task<List<GenericTypeVO>> FindAllAsync();
        public Task<Result> UpdateAsync(GenericTypeVO genericTypeVO);
        public Task<Result> DeleteAsync(int idGeneric);
    }
}
