using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Bakehouse.Core.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        // Props navigation
        public virtual List<Movement> Movements { get; set; }
    }
}