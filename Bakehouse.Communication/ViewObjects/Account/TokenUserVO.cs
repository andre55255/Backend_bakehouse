using System;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class TokenUserVO
    {
        public string Value { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string Error { get; set; }
        public UserVO User { get; set; }
    }
}