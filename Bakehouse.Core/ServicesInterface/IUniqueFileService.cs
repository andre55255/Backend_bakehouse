using Bakehouse.Communication.ViewObjects.Utils;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakehouse.Core.ServicesInterface
{
    public interface IUniqueFileService
    {
        public Result DeleteOneFile(string entityName, string id, string nameFile);

        public Result SaveOneFileBase64(string base64, string entityName, string id, string nameFile);

        public FileVO GetOneFileUrl(string entityName, string id, string nameFile, bool defaultImage = false);

        public Task<FileVO> GetOneFileBase64(string entityName, string id, string nameFile, bool defaultImage = false);
    }
}