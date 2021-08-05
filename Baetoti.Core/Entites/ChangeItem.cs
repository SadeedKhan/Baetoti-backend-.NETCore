using Baetoti.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baetoti.Core.Entites
{
    [Table("ChangeItem", Schema = "baetoti")]
    public class ChangeItem : BaseEntity
    {
        public long ItemId { get; set; }

        public string Name { get; set; }

        public string ArabicName { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public string Reviews { get; set; }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public long UnitID { get; set; }

        public long ProviderID { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public string AveragePreparationTime { get; set; }

        public bool? IsApproved { get; set; }

    }
}
