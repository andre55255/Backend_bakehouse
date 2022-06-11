using System;

namespace Bakehouse.Core.Entities
{
    public class BaseEntities
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
    }
}