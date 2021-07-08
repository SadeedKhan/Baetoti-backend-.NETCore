using Baetoti.Core.Entites.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baetoti.Core.Entites
{
    [Table("Order", Schema = "baetoti")]
    public partial class Order : BaseEntity
    {
        public long CartID { get; set; }
        public long ItemID { get; set; }
        public long Quantity { get; set; }
        public string Comments { get; set; }
    }
}
