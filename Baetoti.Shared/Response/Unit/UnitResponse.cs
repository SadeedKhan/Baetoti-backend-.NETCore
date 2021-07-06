using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.Unit
{
    public class UnitResponse
    {
        public long ID { get; set; }

        public string Family { get; set; }

        public string UnitArabicName { get; set; }

        public string UnitEnglishName { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
    }
}
