using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("Product")]
    public class Product : BaseEntities
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double ValueUnitary { get; set; }
        public double MinQuantity { get; set; }
        public int CategoryId { get; set; }
        public int UnitOfMeasurementId { get; set; }

        // Props navigation
        public Category Category { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public virtual List<OrderPadItem> OrderPadItems { get; set; }
    }
}
