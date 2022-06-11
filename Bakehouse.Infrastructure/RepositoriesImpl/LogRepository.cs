using Bakehouse.Core.Entities;
using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Infrastructure.Data.Context;
using System;

namespace Bakehouse.Infrastructure.RepositoriesImpl
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _db;

        public LogRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Write(string exception, string message, string place)
        {
            try
            {
                Log log = new Log()
                {
                    Exception = exception,
                    Description = message,
                    Object = place,
                    CreatedAt = DateTime.Now
                };

                WriteLog(log);
            }
            catch (Exception) { }
        }

        public void WriteLog(Log log)
        {
            try
            {
                _db.Logs.Add(log);
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }
}