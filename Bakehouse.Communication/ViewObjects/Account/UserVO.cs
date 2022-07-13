using System.Collections.Generic;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class UserVO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Contact { get; set; }
    }
}