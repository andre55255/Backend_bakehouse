using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects
{
    public class GenericTypeVO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome não informado")]
        [StringLength(255, ErrorMessage = "Nome deve ter no máximo 255 caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Token não informado")]
        [StringLength(255, ErrorMessage = "Token deve ter no máximo 255 caracteres", MinimumLength = 3)]
        public string Token { get; set; }

        [Required(ErrorMessage = "Descrição não informado")]
        [StringLength(255, ErrorMessage = "Descrição deve ter no máximo 255 caracteres", MinimumLength = 3)]
        public string Description { get; set; }

        public decimal? Value { get; set; }
    }
}