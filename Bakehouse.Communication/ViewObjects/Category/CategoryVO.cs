using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Category
{
    public class CategoryVO
    {
        [Required(ErrorMessage = "Id não informado")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Descrição não informada")]
        [StringLength(255, ErrorMessage = "Descrição deve conter entre 3 e 255 caracteres", MinimumLength = 3)]
        public string Description { get; set; }
    }
}