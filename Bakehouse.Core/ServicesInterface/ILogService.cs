namespace Bakehouse.Core.ServicesInterface
{
    public interface ILogService
    {
        public void Write(string exception, string message, string place);
    }
}