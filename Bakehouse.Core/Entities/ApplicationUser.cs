using Microsoft.AspNetCore.Identity;
using System;

namespace Bakehouse.Core.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
    }
}