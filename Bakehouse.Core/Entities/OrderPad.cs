using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("OrderPad")]
    public class OrderPad : BaseEntities
    {
        public int Id { get; set; }
        public DateTime DateHour { get; set; }

        // Props navigation
        public virtual List<OrderPadItem> OrderPadItems { get; set; }
    }
}