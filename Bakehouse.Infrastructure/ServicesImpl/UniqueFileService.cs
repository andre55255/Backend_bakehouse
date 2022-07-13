using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.ServicesInterface;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class UniqueFileService : IUniqueFileService
    {
        private readonly IHttpContextAccessor _httpContext;


        public Result DeleteOneFile(string entityName, string id, string nameFile)
        {
            throw new System.NotImplementedException();
        }

        public Task<FileVO> GetOneFileBase64(string entityName, string id, string nameFile)
        {
            throw new System.NotImplementedException();
        }

        public FileVO GetOneFileUrl(string entityName, string id, string nameFile)
        {
            throw new System.NotImplementedException();
        }

        public Result SaveOneFileBase64(string base64, string entityName, string id, string nameFile)
        {
            throw new System.NotImplementedException();
        }
    }
}