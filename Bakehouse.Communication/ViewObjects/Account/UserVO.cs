using Bakehouse.Communication.ViewObjects.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public FileVO ProfileImage { get; set; }
    }

    public class UpdateUserVO
    {
        [Required(ErrorMessage = "Id não informado")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome não informado")]
        [StringLength(255, ErrorMessage = "Nome deve ter entre 3 e 255 caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Sobrenome não informado")]
        [StringLength(255, ErrorMessage = "Sobrenome deve ter entre 3 e 255 caracteres", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Perfis/Roles não informados")]
        public List<string> Roles { get; set; }

        public string Contact { get; set; }

        public FileVO ProfileImage { get; set; }
    }

    public class PrepareUserVO
    {
        public List<SelectObjectVO> Roles { get; set; }
        public UserVO User { get; set; }
    }

    public class SaveUserVO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome não informado")]
        [StringLength(255, ErrorMessage = "Nome deve ter entre 3 e 255 caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Sobrenome não informado")]
        [StringLength(255, ErrorMessage = "Sobrenome deve ter entre 3 e 255 caracteres", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username não informado")]
        [StringLength(256, ErrorMessage = "Username deve ter entre 5 e 256 caracteres", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email não informado")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha não informada")]
        [StringLength(35, ErrorMessage = "Username deve ter entre 6 e 35 caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Perfis/Roles não informados")]
        public List<string> Roles { get; set; }

        public string Contact { get; set; }

        public FileVO ProfileImage { get; set; }
    }
}