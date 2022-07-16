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
    public class FileUniqueService : IFileUniqueService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IGenericTypeRepository _genRepo;
        private readonly ILogService _logService;

        public FileUniqueService(ILogService logService, IHttpContextAccessor httpContext, IGenericTypeRepository genRepo)
        {
            _logService = logService;
            _httpContext = httpContext;
            _genRepo = genRepo;
        }

        /*
          Método para deletar um único arquivo em um diretório, 
            informar entityName=nome da entidade que este arquivo está vinculado(Ex: User)
            informar idEntity=id da entidade que este arquivo está vinculo(Ex: 1)
            informar nameFile=nome base do arquivo salvo(Ex: ProfileImage)
         */
        public Result DeleteFileAtDirectory(string entityName, string idEntity, string nameFile)
        {
            try
            {
                string folder = Path.GetFullPath($"Directory/{entityName}/Unique/{idEntity}/");

                bool verifyFileDeleted = StaticMethods.VerifyFileExistsAndDelete(folder, nameFile);
                if (!verifyFileDeleted)
                    return Result.Fail(ConstantsMessageFileService.ErrorVerifyFileExistsAndDelete);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorDeleteOneFile + entityName,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageFileService.ErrorDeleteOneFile + entityName);
            }
        }

        /*
        Método para retornar FileVO com base64 de um único arquivo em um diretório, 
          informar entityName=nome da entidade que este arquivo está vinculado(Ex: User)
          informar idEntity=id da entidade que este arquivo está vinculo(Ex: 1)
          informar nameFile=nome base do arquivo salvo(Ex: ProfileImage)
          opcional defaultImage=boolean indicando se há uma imagem padrão 
               para retornar caso não haja nenhuma cadastrada para esta entidade
       */
        public async Task<FileVO> GetFileBase64AtDirectory(string entityName, string idEntity, string nameFile, bool defaultImage = false)
        {
            try
            {
                string folder = null;
                if (defaultImage)
                    folder = Path.GetFullPath($"Directory/{entityName}/Default/");
                else
                    folder = Path.GetFullPath($"Directory/{entityName}/Unique/{idEntity}/");

                if (!Directory.Exists(folder))
                    return null;

                string[] files = Directory.GetFiles(folder);
                string file = files.Where(x => x.StartsWith(folder + nameFile.ToUpper())).FirstOrDefault();
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
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorGetFileBase64 + entityName,
                    this.GetType().ToString());

                return null;
            }
        }

        /*
         Método para retornar FileVO contendo url de acesso para um único arquivo em um diretório, 
           informar entityName=nome da entidade que este arquivo está vinculado(Ex: User)
           informar idEntity=id da entidade que este arquivo está vinculo(Ex: 1)
           informar nameFile=nome base do arquivo salvo(Ex: ProfileImage)
           opcional defaultImage=boolean indicando se há uma imagem padrão 
                para retornar caso não haja nenhuma cadastrada para esta entidade
        */
        public FileVO GetFileUrlAtDirectory(string entityName, string idEntity, string nameFile, bool defaultImage = false)
        {
            try
            {
                string folder = null;
                if (defaultImage)
                    folder = Path.GetFullPath($"Directory/{entityName}/Default/");
                else
                    folder = Path.GetFullPath($"Directory/{entityName}/Unique/{idEntity}/");

                if (!Directory.Exists(folder))
                    return null;

                string[] files = Directory.GetFiles(folder);
                string file = files.Where(x => x.StartsWith(folder + nameFile.ToUpper())).FirstOrDefault();
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
                    filePath = string.Concat(baseUrlApi, "/static/", entityName, "/Unique/", idEntity, "/", fileInfo.Name);
                
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
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorGetFileUrl + entityName,
                    this.GetType().ToString());

                return null;
            }
        }
        
        /*
          Método para salvar/editar um único arquivo em um diretório, 
            informar objeto FileVO(name=nome do arquivo, file=base64 dele)
            informar entityName=nome da entidade que este arquivo está vinculado(Ex: User)
            informar idEntity=id da entidade que este arquivo está vinculo(Ex: 1)
         */
        public Result SaveFileBase64AtDirectory(FileVO file, string entityName, string idEntity)
        {
            try
            {
                if (file is null || string.IsNullOrEmpty(file.File))
                    return Result.Fail(ConstantsMessageFileService.ErrorBase64NotFound + entityName);

                string folder = Path.GetFullPath($"Directory/{entityName}/Unique/{idEntity}/");
                bool verifyFileDeleted = StaticMethods.VerifyFileExistsAndDelete(folder, file.Name.ToUpper());
                if (!verifyFileDeleted)
                    return Result.Fail(ConstantsMessageFileService.ErrorVerifyFileExistsAndDelete);

                string imageNameSave = StaticMethods.SaveFileDirectoryFromBase64(folder, file.File, file.Name.ToUpper());
                if (imageNameSave is null)
                    return Result.Fail(ConstantsMessageFileService.ErrorSaveOneFileGeneric + entityName);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorSaveOneFileGeneric + entityName,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageFileService.ErrorSaveOneFileGeneric + entityName);
            }
        }
    }
}