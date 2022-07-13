using AutoMapper;
using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly IUniqueFileService _uniqueFileService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public UserService(IApplicationUserRepository userRepo, ILogService logService, IMapper mapper, IUniqueFileService uniqueFileService)
        {
            _userRepo = userRepo;
            _logService = logService;
            _mapper = mapper;
            _uniqueFileService = uniqueFileService;
        }

        public async Task<Result> CreateAsync(SaveUserVO userVO)
        {
            try
            {
                ApplicationUser hasUserEmail = await _userRepo.FindByEmailAsync(userVO.Email);
                if (hasUserEmail is not null)
                    return Result.Fail(ConstantsMessageUsers.ErrorUserEmailExists);

                ApplicationUser hasUserName = await _userRepo.FindByUserNameAsync(userVO.UserName);
                if (hasUserName is not null)
                    return Result.Fail(ConstantsMessageUsers.ErrorUserNameExists);

                ApplicationUser userSave = _mapper.Map<ApplicationUser>(userVO);

                Result resultCreated = await _userRepo.CreateUserDbAsync(userSave, userVO.Roles);
                if (resultCreated.IsFailed)
                    return resultCreated;

                string idCreated = resultCreated.Successes.FirstOrDefault().Message;
                if (userVO.ProfileImage is not null &&
                    string.IsNullOrEmpty(userVO.ProfileImage.File) &&
                    idCreated is not null)
                {
                    Result resultSaveProfileImage =
                        _uniqueFileService.SaveOneFileBase64(userVO.ProfileImage.File,
                                                             "ApplicationUser",
                                                             idCreated,
                                                             "Profile");

                    if (resultSaveProfileImage.IsFailed)
                        return resultSaveProfileImage;
                }

                return Result.Ok().WithSuccess(idCreated);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorCreateUser,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUsers.ErrorCreateUser);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                ApplicationUser user = await _userRepo.FindByIdAsync(id);
                if (user is null)
                    return Result.Fail(ConstantsMessageUsers.ErrorUserNotFound);

                Result result = await _userRepo.DisableUserAsync(user);
                return result;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorDisableUser,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUsers.ErrorDisableUser);
            }
        }

        public async Task<List<UserVO>> GetAllAsync()
        {
            try
            {
                List<ApplicationUser> usersSave = await _userRepo.GetAllUsersAsync();
                if (usersSave is null)
                    return null;

                List<UserVO> usersVO = _mapper.Map<List<UserVO>>(usersSave);
                foreach (UserVO user in usersVO)
                {
                    ApplicationUser getEntityUser = usersSave.Where(x => x.Id == user.Id)
                                                             .FirstOrDefault();

                    List<string> rolesUser = await _userRepo.GetRolesFromUserAsync(getEntityUser);
                    user.Roles = rolesUser;

                    FileVO profileImg =
                        _uniqueFileService.GetOneFileUrl("ApplicationUser", user.Id.ToString(), "Profile");

                    if (profileImg is null)
                        profileImg =
                            _uniqueFileService.GetOneFileUrl("ApplicationUser", user.Id.ToString(), "profile_default", true);

                    user.ProfileImage = profileImg;
                }

                return usersVO;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorGetAllUsers,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<UserVO> GetByIdAsync(int id)
        {
            try
            {
                ApplicationUser userSave = await _userRepo.FindByIdAsync(id);
                if (userSave is null)
                    return null;

                UserVO response = _mapper.Map<UserVO>(userSave);

                FileVO profileImg =
                        _uniqueFileService.GetOneFileUrl("ApplicationUser", userSave.Id.ToString(), "Profile");

                if (profileImg is null)
                    profileImg =
                        _uniqueFileService.GetOneFileUrl("ApplicationUser", userSave.Id.ToString(), "profile_default", true);

                response.ProfileImage = profileImg;

                List<string> rolesUser = await _userRepo.GetRolesFromUserAsync(userSave);
                response.Roles = rolesUser;

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorGetById,
                    this.GetType().ToString());

                return null;
            }
        }

        public async Task<Result> UpdateAsync(UpdateUserVO userVO)
        {
            try
            {
                ApplicationUser userSave = _mapper.Map<ApplicationUser>(userVO);

                Result resultEdit = await _userRepo.UpdateUserAsync(userSave, userVO.Roles);
                if (resultEdit.IsFailed)
                    return resultEdit;

                string idEdited = resultEdit.Successes.FirstOrDefault().Message;
                if (userVO.ProfileImage is not null &&
                    userVO.ProfileImage.Disable)
                {
                    Result resultDeleteProfileImg =
                        _uniqueFileService.DeleteOneFile("ApplicationUser", idEdited, "Profile");

                    if (resultDeleteProfileImg.IsFailed)
                        return resultDeleteProfileImg;
                }
                else if (userVO.ProfileImage is not null &&
                    string.IsNullOrEmpty(userVO.ProfileImage.File) &&
                    idEdited is not null)
                {
                    Result resultSaveProfileImage =
                        _uniqueFileService.SaveOneFileBase64(userVO.ProfileImage.File,
                                                             "ApplicationUser",
                                                             idEdited,
                                                             "Profile");

                    if (resultSaveProfileImage.IsFailed)
                        return resultSaveProfileImage;
                }

                return Result.Ok().WithSuccess(idEdited);
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message,
                    ConstantsMessageUsers.ErrorUpdateUser,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageUsers.ErrorUpdateUser);
            }
        }
    }
}