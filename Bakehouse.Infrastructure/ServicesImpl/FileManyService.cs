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
    public class FileManyService : IFileManyService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IGenericTypeRepository _genRepo;
        private readonly ILogService _logService;

        public FileManyService(IHttpContextAccessor httpContext, IGenericTypeRepository genRepo, ILogService logService)
        {
            _httpContext = httpContext;
            _genRepo = genRepo;
            _logService = logService;
        }

        /*
           Método para criar/editar muitos arquivos em um diretório
           Este método deleta todos os arquivos do diretório se existir e salva os novos
             informar galleryFiles=Um List<FileVO> contendo os arquivos comuns a serem salvos
             opcional fileMain=Um FileVO que vai conter o arquivo principal salvo, 
                um arquivo principal que ficará em destaque
             informar entityName=nome da entidade que estes arquivos estão vinculados(Ex: User)
             informar idEntity=id da entidade que estes arquivos estão vinculados(Ex: 1)
         */
        public FileManySaveVO SaveFileBase64AtDirectory(List<FileVO> galleryFiles, FileVO fileMain, string entityName, string idEntity)
        {
            try
            {
                FileManySaveVO response = new FileManySaveVO();
                string folder = Path.GetFullPath($"Directory/{entityName}/Many/{idEntity}/");

                if (Directory.Exists(folder))
                {
                    string[] filesDir = Directory.GetFiles(folder);
                    foreach (string file in filesDir)
                    {
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                }

                if (fileMain != null)
                {
                    string fileNameSave = StaticMethods.SaveFileDirectoryFromBase64(folder, fileMain.File, "MAIN");
                    if (fileNameSave is null)
                        return null;

                    response.MainName += fileNameSave;
                }

                foreach (FileVO file in galleryFiles)
                {
                    string fileNameSave = StaticMethods.SaveFileDirectoryFromBase64(folder, file.File, file.Name);
                    if (fileNameSave is null)
                        return null;

                    response.GalleryName += fileNameSave;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorListSaveFileGeneric + entityName,
                    this.GetType().ToString());

                return null;
            }
        }

        /*
           Método para retornar uma lista de FileVO contendo a url de acesso para arquivos de um diretório
             informar galleryFiles=Um array de string contendo os nomes dos arquivos de galeria salvos
             opcional mainFile=Uma string que vai conter o nome arquivo principal salvo, 
                um arquivo principal que ficará em destaque
             informar entityName=nome da entidade que estes arquivos estão vinculados(Ex: User)
             informar idEntity=id da entidade que estes arquivos estão vinculados(Ex: 1)
         */
        public List<FileVO> GetFilesDirectoryToUrl(string[] galleryFiles, string mainFile, string entityName, string idEntity)
        {
            try
            {
                List<FileVO> response = new List<FileVO>();
                string folder = Path.GetFullPath($"Directory/{entityName}/Many/{idEntity}/");
                if (!Directory.Exists(folder))
                    return null;

                string[] files = Directory.GetFiles(folder);
                string mainFileDir = files.Where(x => x.StartsWith(folder + mainFile))
                                          .FirstOrDefault();

                if (mainFileDir is not null && File.Exists(mainFileDir))
                {
                    string urlMainFile = BuildUrlFile(mainFileDir, idEntity, entityName);
                    if (urlMainFile is null)
                        return null;

                    FileVO fileMainVO = new FileVO
                    {
                        Name = mainFile,
                        File = urlMainFile
                    };
                    response.Add(fileMainVO);
                }

                foreach (string filename in galleryFiles)
                {
                    string filePathDirectory = files.Where(x => x.StartsWith(folder + filename))
                                                    .FirstOrDefault();

                    if (filePathDirectory is not null && File.Exists(filePathDirectory))
                    {
                        string urlFile = BuildUrlFile(filePathDirectory, idEntity, entityName);
                        if (urlFile is null)
                            return null;

                        FileVO fileVO = new FileVO
                        {
                            Name = mainFile,
                            File = urlFile
                        };
                        response.Add(fileVO);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorListGetFilesBase64 + entityName,
                   this.GetType().ToString());

                return null;
            }
        }

        /*
           Método para deletar os arquivos informados de um diretório de uma entidade específica
             informar galleryFiles=Um array de string contendo os nomes dos arquivos de galeria a serem deletados
             opcional mainFile=Uma string que vai conter o nome arquivo principal a ser deletado, 
             informar entityName=nome da entidade que estes arquivos estão vinculados(Ex: User)
             informar idEntity=id da entidade que estes arquivos estão vinculos(Ex: 1)
         */
        public Result DeleteFilesDirectory(string[] galleryFiles, string mainFile, string entityName, string idEntity)
        {
            try
            {
                string folder = Path.GetFullPath($"Directory/{entityName}/Many/{idEntity}/");
                if (!Directory.Exists(folder))
                    return Result.Fail(ConstantsMessageFileService.ErrorDirectoryNotFound + folder);

                if (mainFile != null)
                {
                    bool hasDeleted = StaticMethods.VerifyFileExistsAndDelete(folder, mainFile);
                    if (!hasDeleted)
                        return Result.Fail(ConstantsMessageFileService.ErrorVerifyFileExistsAndDelete);
                }

                foreach (string filename in galleryFiles)
                {
                    bool hasDeleted = StaticMethods.VerifyFileExistsAndDelete(folder, filename);
                    if (!hasDeleted)
                        return Result.Fail(ConstantsMessageFileService.ErrorVerifyFileExistsAndDelete);
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorDeleteManyFile + entityName,
                    this.GetType().ToString());

                return Result.Fail(ConstantsMessageFileService.ErrorDeleteManyFile + entityName);
            }
        }

        /*
           Método para retornar uma lista de FileVO contendo um base64 dos arquivos de um diretório
             informar galleryFiles=Um array de string contendo os nomes dos arquivos de galeria salvos
             opcional mainFile=Uma string que vai conter o nome arquivo principal salvo, 
                um arquivo principal que ficará em destaque
             informar entityName=nome da entidade que estes arquivos estão vinculados(Ex: User)
             informar idEntity=id da entidade que estes arquivos estão vinculados(Ex: 1)
         */
        public async Task<List<FileVO>> GetFilesDirectoryToBase64(string[] galleryFiles, string mainFile, string entityName, string idEntity)
        {
            try
            {
                List<FileVO> response = new List<FileVO>();
                string folder = Path.GetFullPath($"Directory/{entityName}/Many/{idEntity}/");
                if (!Directory.Exists(folder))
                    return null;

                string[] files = Directory.GetFiles(folder);
                string mainFileDir = files.Where(x => x.StartsWith(folder + mainFile))
                                          .FirstOrDefault();

                if (mainFileDir is not null && File.Exists(mainFileDir))
                {
                    string base64MainFile = await BuildBase64File(mainFileDir);
                    if (base64MainFile is null)
                        return null;

                    FileVO fileMainVO = new FileVO
                    {
                        Name = mainFile,
                        File = base64MainFile
                    };
                    response.Add(fileMainVO);
                }

                foreach (string filename in galleryFiles)
                {
                    string filePathDirectory = files.Where(x => x.StartsWith(folder + filename))
                                                    .FirstOrDefault();

                    if (filePathDirectory is not null && File.Exists(filePathDirectory))
                    {
                        string base64File = await BuildBase64File(filePathDirectory);
                        if (base64File is null)
                            return null;

                        FileVO fileVO = new FileVO
                        {
                            Name = mainFile,
                            File = base64File
                        };
                        response.Add(fileVO);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorListGetFilesBase64 + entityName,
                   this.GetType().ToString());

                return null;
            }
        }

        private string BuildUrlFile(string filePath, string idEntity, string entityName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                HttpRequest request = _httpContext.HttpContext.Request;
                string baseUrlApi = $"{request.Scheme}://{request.Host}";

                string urlFile = string.Concat(baseUrlApi, "/static/", entityName, "/Many/", idEntity, "/", fileInfo.Name);
                return urlFile;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorBuildUrlFile + filePath,
                   this.GetType().ToString());

                return null;
            }
        }

        private async Task<string> BuildBase64File(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string tokenGenType = "BASE64_" + fileInfo.Extension.Split('.')[1];

                List<GenericType> genTypeHeaderBase64 = await _genRepo.FindByTokenAsync(tokenGenType.ToUpper());
                if (genTypeHeaderBase64 is null)
                    return null;

                byte[] fileBytes = File.ReadAllBytes(filePath);
                string fileBase64 = Convert.ToBase64String(fileBytes);

                string base64WithHeader = genTypeHeaderBase64.FirstOrDefault().Name + "," + fileBase64;
                return base64WithHeader;
            }
            catch (Exception ex)
            {
                _logService.Write(ex.Message, ConstantsMessageFileService.ErrorBuildBase64File + filePath,
                   this.GetType().ToString());

                return null;
            }
        }
    }
}