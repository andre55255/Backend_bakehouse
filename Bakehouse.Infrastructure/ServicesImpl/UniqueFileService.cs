using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using Bakehouse.Helpers;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class UniqueFileService : IUniqueFileService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IGenericTypeRepository _genRepo;
        private readonly ILogService _logService;

        public UniqueFileService(IHttpContextAccessor httpContext, IGenericTypeRepository genRepo, ILogService logService)
        {
            _httpContext = httpContext;
            _genRepo = genRepo;
            _logService = logService;
        }

        public Result DeleteOneFile(string entityName, string id, string nameFile)
        {
            try
            {
                string folder = Path.GetFullPath($"Directory/{entityName}/Unique/{id}");

                bool verifyFileDeleted = VerifyFileExistsAndDelete(folder, nameFile);
                if (!verifyFileDeleted)
                    return Result.Fail(ConstantsMessagesFileService.ErrorVerifyFileExistsAndDelete);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessagesFileService.ErrorDeleteOneFile + entityName,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessagesFileService.ErrorDeleteOneFile + entityName);
            }
        }

        public async Task<FileVO> GetOneFileBase64(string entityName, string id, string nameFile, bool defaultImage = false)
        {
            try
            {
                string folder = null;
                if (defaultImage)
                    folder = Path.GetFullPath($"Directory/{entityName}/Default/");
                else
                    folder = Path.GetFullPath($"Directory/{entityName}/Unique/{id}/");

                if (!Directory.Exists(folder))
                    return null;

                string[] files = Directory.GetFiles(folder);
                string file = files.Where(x => x.StartsWith(folder + nameFile)).FirstOrDefault();
                if (file is null)
                    return null;

                if (!File.Exists(file))
                    return null;

                FileInfo fileInfo = new FileInfo(file);
                string tokenGenType = "BASE64_" + fileInfo.Extension.Split('.')[1];

                List<GenericType> genTypeHeaderBase64 = await _genRepo.FindByTokenAsync(tokenGenType.ToUpper());
                if (genTypeHeaderBase64 is null)
                    return null;

                byte[] fileBytes = File.ReadAllBytes(file);
                string fileBase64 = Convert.ToBase64String(fileBytes);

                FileVO response = new FileVO
                {
                    File = genTypeHeaderBase64.FirstOrDefault().Name + "," + fileBase64,
                    Name = fileInfo.Name
                };

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessagesFileService.ErrorGetFileBase64 + entityName,
                    this.GetType().ToString());

                return null;
            }
        }

        public FileVO GetOneFileUrl(string entityName, string id, string nameFile, bool defaultImage = false)
        {
            try
            {
                string folder = null;
                if (defaultImage)
                    folder = Path.GetFullPath($"Directory/{entityName}/Default/");
                else
                    folder = Path.GetFullPath($"Directory/{entityName}/Unique/{id}/");

                if (!Directory.Exists(folder))
                    return null;

                string[] files = Directory.GetFiles(folder);
                string file = files.Where(x => x.StartsWith(folder + nameFile)).FirstOrDefault();
                if (file is null)
                    return null;

                if (!File.Exists(file))
                    return null;

                FileInfo fileInfo = new FileInfo(file);
                HttpRequest request = _httpContext.HttpContext.Request;
                string baseUrlApi = $"{request.Scheme}://{request.Host}";

                string filePath = null;
                if (defaultImage)
                    filePath = string.Concat(baseUrlApi, "/static/", entityName, "/Default/", fileInfo.Name);
                else
                    filePath = string.Concat(baseUrlApi, "/static/", entityName, "/Unique/", id, "/", fileInfo.Name);

                string fileName = fileInfo.Name;

                FileVO response = new FileVO
                {
                    File = filePath,
                    Name = fileName
                };
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessagesFileService.ErrorGetFileUrl + entityName,
                    this.GetType().ToString());

                return null;
            }
        }

        public Result SaveOneFileBase64(string base64, string entityName, string id, string nameFile)
        {
            try
            {
                if (base64 is null)
                    return Result.Fail(ConstantsMessagesFileService.ErrorBase64NotFound + entityName);

                string folder = Path.GetFullPath($"Directory/{entityName}/Unique/{id}/");
                bool verifyFileDeleted = VerifyFileExistsAndDelete(folder, nameFile);
                if (!verifyFileDeleted)
                    return Result.Fail(ConstantsMessagesFileService.ErrorVerifyFileExistsAndDelete);

                string imageNameSave = SaveFileDirectoryFromBase64(folder, base64, nameFile);
                if (imageNameSave is null)
                    return Result.Fail(ConstantsMessagesFileService.ErrorSaveOneFileGeneric + entityName);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessagesFileService.ErrorSaveOneFileGeneric + entityName,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessagesFileService.ErrorSaveOneFileGeneric + entityName);
            }
        }

        private bool VerifyFileExistsAndDelete(string folder, string nameFile)
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    string[] files = Directory.GetFiles(folder);
                    string file = files.Where(x => x.StartsWith(folder + nameFile)).FirstOrDefault();
                    if (file is null)
                        return false;

                    if (!File.Exists(file))
                        return false;

                    File.Delete(file);
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                   ConstantsMessagesFileService.ErrorVerifyFileExistsAndDelete,
                   this.GetType().ToString());

                return false;
            }
        }

        private string SaveFileDirectoryFromBase64(string folder, string base64, string nameFile)
        {
            try
            {
                int index = base64.IndexOf(",");
                string base64file = base64.Remove(0, index + 1);

                index = base64.IndexOf(";");
                string base64type = base64.Substring(0, index);
                index = base64.IndexOf("/");

                string extension = base64type.Substring(index + 1);

                byte[] bytes = Convert.FromBase64String(base64file);

                DateTime today = DateTime.Now;
                string suffix = string.Concat("_", Guid.NewGuid().ToString());
                string name = string.Concat(nameFile, suffix);

                string path = Path.Combine(folder, name);

                if (File.Exists(path))
                    File.Delete(path);

                Directory.CreateDirectory(folder);
                File.WriteAllBytes(path + "." + extension, bytes);

                return name + "." + extension;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, 
                    ConstantsMessagesFileService.ErrorConvertAndSaveBase64Directory + folder,
                    this.GetType().ToString());

                return null;
            }
        }
    }
}