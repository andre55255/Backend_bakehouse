using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("Log")]
    public class Log
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Object { get; set; }
        public string Exception { get; set; }
        public string Description { get; set; }
        public string ResponseJson { get; set; }
    }
}