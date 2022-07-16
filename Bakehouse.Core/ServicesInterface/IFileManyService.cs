using Bakehouse.Communication.ViewObjects.Utils;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IFileManyService
    {
        public FileManySaveVO SaveFileBase64AtDirectory(List<FileVO> galleryFiles, FileVO fileMain, string entityName, string idEntity);

        public List<FileVO> GetFilesDirectoryToUrl(string[] galleryFiles, string mainFile, string entityName, string idEntity);

        public Result DeleteFilesDirectory(string[] galleryFiles, string mainFile, string entityName, string idEntity);

        public Task<List<FileVO>> GetFilesDirectoryToBase64(string[] galleryFiles, string mainFile, string entityName, string idEntity);
    }
}