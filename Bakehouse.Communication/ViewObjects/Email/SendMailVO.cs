using System.Collections.Generic;

namespace Bakehouse.Communication.ViewObjects.Email
{
    public class SendMailVO
    {
        public SendMailVO()
        {
            Destinations = new List<string>();
        }

        public string Destination { get; set; }
        public List<string> Destinations { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
