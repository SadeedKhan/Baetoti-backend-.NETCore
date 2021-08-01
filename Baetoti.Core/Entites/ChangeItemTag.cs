using Baetoti.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baetoti.Core.Entites
{
    [Table("TempItemTag", Schema = "baetoti")]
    public class ChangeItemTag : BaseEntity
    {
        public long ItemID { get; set; }

        public long TagID { get; set; }

    }
}
