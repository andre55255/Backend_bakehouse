using Bakehouse.Core.Entities;

namespace Bakehouse.Core.RepositoriesInterface
{
    public interface ILogRepository
    {
        public void Write(string exception, string message, string place);
        public void WriteLog(Log log);
    }
}