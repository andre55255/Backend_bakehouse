using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class LoginVO
    {
        [Required(ErrorMessage = "Nome de usuário é obrigatório")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; }
    }
}