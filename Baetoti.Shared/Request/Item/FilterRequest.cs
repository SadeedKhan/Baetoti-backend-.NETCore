using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Request.Item
{
    public class FilterRequest
    {
        public string Name { get; set; }

        public string ArabicName { get; set; }

        public decimal? Price { get; set; }

        public long? CategoryID { get; set; }

        public long? SubCategoryID { get; set; }

        public long? UnitID { get; set; }

        public long? TagID { get; set; }
    }
}
