using System.ComponentModel.DataAnnotations;

namespace Bakehouse.Communication.ViewObjects.Account
{
    public class ConfirmAccountVO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}