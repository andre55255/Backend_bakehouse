using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("Movement")]
    public class Movement : BaseEntities
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateHour { get; set; }
        public double TotalValue { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }

        // PropsNavigation
        public GenericType Type { get; set; }
        public ApplicationUser User { get; set; }
    }
}