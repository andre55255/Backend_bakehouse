using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class RefreshTokenVO
    {
        [Required(ErrorMessage = "Informe o campo de token")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Informe o campo de refreshToken")]
        public string RefreshToken { get; set; }
    }
}