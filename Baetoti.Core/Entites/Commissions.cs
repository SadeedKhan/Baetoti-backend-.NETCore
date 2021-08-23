using Baetoti.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Core.Entites
{
    public class Commissions : BaseEntity
    {
        public decimal Commission { get; set; }
        public int UserType { get; set; }
        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public bool MarkAsDeleted { get; set; }

    }
}
