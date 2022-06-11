using System.ComponentModel.DataAnnotations.Schema;

namespace Bakehouse.Core.Entities
{
    [Table("Configuration")]
    public class Configuration : BaseEntities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Value { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
    }
}