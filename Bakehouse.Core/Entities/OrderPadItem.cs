using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("OrderPadItem")]
    public class OrderPadItem : BaseEntities
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public double ValueUnitary { get; set; }
        public int ProductId { get; set; }
        public int OrderPadId { get; set; }

        // Props navigation
        public Product Product { get; set; }
        public OrderPad OrderPad { get; set; }
    }
}