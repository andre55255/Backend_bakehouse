using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class ResetPasswordVO
    {
        [Required(ErrorMessage = "Email não informado")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nova senha não informada")]
        [DataType(DataType.Password, ErrorMessage = "Senha inválida")]
        [StringLength(35, ErrorMessage = "A senha deve ter no máximo 35 caracteres e no mínimo 6", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Código/Token de redefinição de senha não informado")]
        public string Code { get; set; }
    }
}