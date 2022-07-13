using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class ResetPasswordSignInVO
    {
        [Required(ErrorMessage = "Nova senha não informada")]
        [DataType(DataType.Password, ErrorMessage = "Senha inválida")]
        [StringLength(35, ErrorMessage = "A senha deve ter no máximo 35 caracteres e no mínimo 6", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}