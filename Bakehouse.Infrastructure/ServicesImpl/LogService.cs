using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Core.ServicesInterface;
using System;

namespace Bakehouse.Infrastructure.ServicesImpl
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        public void Write(string exception, string message, string place)
        {
            try
            {
                _logRepo.Write(exception, message, place);
            }
            catch (Exception) { }
        }
    }
}