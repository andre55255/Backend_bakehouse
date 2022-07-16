using Bakehouse.Communication.ViewObjects.Utils;
using FluentResults;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IFileUniqueService
    {
        public Result DeleteFileAtDirectory(string entityName, string idEntity, string nameFile);

        public Result SaveFileBase64AtDirectory(FileVO file, string entityName, string idEntity);

        public FileVO GetFileUrlAtDirectory(string entityName, string idEntity, string nameFile, bool defaultImage = false);
        
        public Task<FileVO> GetFileBase64AtDirectory(string entityName, string idEntity, string nameFile, bool defaultImage = false);
    }
}