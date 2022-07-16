using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Communication.ViewObjects.Utils;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IUserService
    {
        public Task<Result> CreateAsync(SaveUserVO userVO);

        public Task<Result> UpdateAsync(UpdateUserVO userVO);

        public Task<UserVO> GetByIdAsync(int id);

        public Task<List<UserVO>> GetAllAsync();

        public Task<Result> DeleteAsync(int id);

        public Task<List<SelectObjectVO>> GetRolesAsSelectObject();
    }
}