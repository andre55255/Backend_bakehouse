using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Configuration
{
    public class ConfigurationVO
    {
        [Required(ErrorMessage = "Id é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(256, ErrorMessage = "Nome deve ter no máximo 256 caracteres", MinimumLength = 3)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Token é obrigatório")]
        [StringLength(256, ErrorMessage = "Token deve ter no máximo 256 caracteres", MinimumLength = 3)]
        public string Token { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório")]
        [StringLength(256, ErrorMessage = "Valor deve ter no máximo 256 caracteres", MinimumLength = 0)]
        public string Value { get; set; }

        [StringLength(256, ErrorMessage = "Extra1 deve ter no máximo 256 caracteres", MinimumLength = 0)]
        public string Extra1 { get; set; }

        [StringLength(256, ErrorMessage = "Extra2 deve ter no máximo 256 caracteres", MinimumLength = 0)]
        public string Extra2 { get; set; }
    }
}