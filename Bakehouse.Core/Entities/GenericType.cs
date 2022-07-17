using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("GenericType")]
    public class GenericType : BaseEntities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }

        // Props navigation
        public virtual List<Movement> Movements { get; set; }
    }
}