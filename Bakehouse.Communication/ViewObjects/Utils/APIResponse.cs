namespace Bakehouse.Communication.ViewObjects.Utils
{
    public class APIResponse
    {
        public bool Success { get; set; }
        public int Number { get; set; }
        public object Object { get; set; }
        public string Message { get; set; }
    }
}