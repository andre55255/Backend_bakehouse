using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("Category")]
    public class Category : BaseEntities
    {
        public int Id { get; set; }
        public string Description { get; set; }

        // Props navigation
        public virtual List<Product> Products { get; set; }
    }
}